/**
 * MERGE BOUNCE - Firebase Cloud Functions Backend
 * Complete backend infrastructure for mobile game
 */

const functions = require('firebase-functions');
const admin = require('firebase-admin');
const express = require('express');
const cors = require('cors');

admin.initializeApp();
const db = admin.firestore();
const auth = admin.auth();

// ============================================================================
// AUTHENTICATION SYSTEM
// ============================================================================

/**
 * Create user profile after authentication
 * Triggered automatically when new user signs up
 */
exports.createUserProfile = functions.auth.user().onCreate(async (user) => {
  const userProfile = {
    uid: user.uid,
    email: user.email,
    displayName: user.displayName || `Player${user.uid.substring(0, 6)}`,
    photoURL: user.photoURL || null,
    createdAt: admin.firestore.FieldValue.serverTimestamp(),
    lastLogin: admin.firestore.FieldValue.serverTimestamp(),

    // Game stats
    level: 1,
    experience: 0,
    totalGamesPlayed: 0,
    highScore: 0,
    totalMerges: 0,
    highestCombo: 0,
    extremeFeverCount: 0,

    // Currency
    coins: 0,
    gems: 0,

    // Progression
    unlockedSkins: ['default'],
    upgrades: {
      ballTrail: 0,
      comboWindow: 0,
      powerupFrequency: 0,
      startingBall: 0
    },

    // Premium
    isPremium: false,
    premiumExpiryDate: null,

    // Social
    clanId: null,
    friendCount: 0,

    // Misc
    tutorialCompleted: false,
    dailyLoginStreak: 0,
    lastDailyLogin: null
  };

  try {
    await db.collection('users').doc(user.uid).set(userProfile);
    console.log(`Created profile for user ${user.uid}`);
  } catch (error) {
    console.error('Error creating user profile:', error);
  }
});

/**
 * Update last login timestamp
 */
exports.updateLastLogin = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;

  try {
    const userRef = db.collection('users').doc(userId);
    const userDoc = await userRef.get();
    const userData = userDoc.data();

    const now = new Date();
    const lastLogin = userData.lastDailyLogin ? userData.lastDailyLogin.toDate() : null;

    let streak = userData.dailyLoginStreak || 0;

    // Check if last login was yesterday (streak continues)
    if (lastLogin) {
      const diffDays = Math.floor((now - lastLogin) / (1000 * 60 * 60 * 24));
      if (diffDays === 1) {
        streak += 1; // Continue streak
      } else if (diffDays > 1) {
        streak = 1; // Streak broken, start new
      }
      // diffDays === 0 means same day, no change
    } else {
      streak = 1; // First login
    }

    await userRef.update({
      lastLogin: admin.firestore.FieldValue.serverTimestamp(),
      lastDailyLogin: admin.firestore.FieldValue.serverTimestamp(),
      dailyLoginStreak: streak
    });

    return { success: true, streak: streak };
  } catch (error) {
    console.error('Error updating last login:', error);
    throw new functions.https.HttpsError('internal', 'Failed to update login');
  }
});

// ============================================================================
// CLOUD SAVE SYSTEM
// ============================================================================

/**
 * Save player progress to cloud
 */
exports.saveProgress = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const gameData = data;

  try {
    const saveData = {
      coins: gameData.coins || 0,
      gems: gameData.gems || 0,
      highScore: gameData.highScore || 0,
      level: gameData.level || 1,
      experience: gameData.experience || 0,
      unlockedSkins: gameData.unlockedSkins || ['default'],
      upgrades: gameData.upgrades || {},
      stats: {
        totalGamesPlayed: gameData.totalGamesPlayed || 0,
        totalMerges: gameData.totalMerges || 0,
        highestCombo: gameData.highestCombo || 0,
        extremeFeverCount: gameData.extremeFeverCount || 0
      },
      lastSaved: admin.firestore.FieldValue.serverTimestamp()
    };

    await db.collection('saves').doc(userId).set(saveData, { merge: true });
    await db.collection('users').doc(userId).update(saveData);

    return { success: true, timestamp: Date.now() };
  } catch (error) {
    console.error('Error saving progress:', error);
    throw new functions.https.HttpsError('internal', 'Failed to save progress');
  }
});

/**
 * Load player progress from cloud
 */
exports.loadProgress = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;

  try {
    const saveDoc = await db.collection('saves').doc(userId).get();

    if (!saveDoc.exists) {
      // No save data, return defaults
      return {
        coins: 0,
        gems: 0,
        highScore: 0,
        level: 1,
        experience: 0,
        unlockedSkins: ['default'],
        upgrades: {},
        stats: {}
      };
    }

    return saveDoc.data();
  } catch (error) {
    console.error('Error loading progress:', error);
    throw new functions.https.HttpsError('internal', 'Failed to load progress');
  }
});

// ============================================================================
// LEADERBOARD SYSTEM
// ============================================================================

/**
 * Submit score to leaderboards with anti-cheat validation
 */
exports.submitScore = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { score, gameData } = data;

  // Anti-cheat validation
  const isValid = validateScore(score, gameData);
  if (!isValid) {
    console.warn(`Suspicious score from user ${userId}: ${score}`);
    // Flag for review but don't reject immediately
    await db.collection('suspicious_scores').add({
      userId: userId,
      score: score,
      gameData: gameData,
      timestamp: admin.firestore.FieldValue.serverTimestamp()
    });
  }

  try {
    const userDoc = await db.collection('users').doc(userId).get();
    const userData = userDoc.data();

    // Update all-time leaderboard
    const leaderboardEntry = {
      userId: userId,
      score: score,
      displayName: userData.displayName,
      level: userData.level,
      timestamp: admin.firestore.FieldValue.serverTimestamp()
    };

    await db.collection('leaderboards').doc('allTime').collection('scores').doc(userId).set(leaderboardEntry);

    // Update daily leaderboard
    const today = getTodayKey();
    await db.collection('leaderboards').doc('daily').collection(today).doc(userId).set(leaderboardEntry);

    // Update weekly leaderboard
    const week = getWeekKey();
    await db.collection('leaderboards').doc('weekly').collection(week).doc(userId).set(leaderboardEntry);

    // Update user's high score if better
    if (score > (userData.highScore || 0)) {
      await db.collection('users').doc(userId).update({
        highScore: score
      });
    }

    // Get rank
    const rank = await calculateRank(userId, score, 'allTime');

    return { success: true, rank: rank, score: score };
  } catch (error) {
    console.error('Error submitting score:', error);
    throw new functions.https.HttpsError('internal', 'Failed to submit score');
  }
});

/**
 * Get top scores from leaderboard
 */
exports.getLeaderboard = functions.https.onCall(async (data, context) => {
  const { type = 'allTime', limit = 100 } = data;

  try {
    let collection;
    if (type === 'allTime') {
      collection = db.collection('leaderboards').doc('allTime').collection('scores');
    } else if (type === 'daily') {
      const today = getTodayKey();
      collection = db.collection('leaderboards').doc('daily').collection(today);
    } else if (type === 'weekly') {
      const week = getWeekKey();
      collection = db.collection('leaderboards').doc('weekly').collection(week);
    }

    const snapshot = await collection
      .orderBy('score', 'desc')
      .limit(limit)
      .get();

    const scores = [];
    let rank = 1;
    snapshot.forEach(doc => {
      scores.push({
        rank: rank++,
        ...doc.data()
      });
    });

    return { scores: scores };
  } catch (error) {
    console.error('Error getting leaderboard:', error);
    throw new functions.https.HttpsError('internal', 'Failed to get leaderboard');
  }
});

/**
 * Get player's rank
 */
exports.getPlayerRank = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { type = 'allTime' } = data;

  try {
    const userDoc = await db.collection('users').doc(userId).get();
    const userScore = userDoc.data().highScore || 0;

    const rank = await calculateRank(userId, userScore, type);

    return {
      rank: rank,
      score: userScore
    };
  } catch (error) {
    console.error('Error getting player rank:', error);
    throw new functions.https.HttpsError('internal', 'Failed to get rank');
  }
});

// ============================================================================
// MATCHMAKING & PVP SYSTEM
// ============================================================================

/**
 * Join matchmaking queue
 */
exports.joinMatchmaking = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { skillRating = 1000 } = data;

  try {
    // Add to queue
    const queueEntry = {
      userId: userId,
      skillRating: skillRating,
      joinedAt: admin.firestore.FieldValue.serverTimestamp(),
      status: 'searching'
    };

    await db.collection('matchmaking').doc('queue').collection('players').doc(userId).set(queueEntry);

    // Try to find match
    const match = await findMatch(userId, skillRating);

    if (match) {
      return { matched: true, matchData: match };
    } else {
      return { matched: false, message: 'Searching for opponent...' };
    }
  } catch (error) {
    console.error('Error joining matchmaking:', error);
    throw new functions.https.HttpsError('internal', 'Failed to join matchmaking');
  }
});

/**
 * Leave matchmaking queue
 */
exports.leaveMatchmaking = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;

  try {
    await db.collection('matchmaking').doc('queue').collection('players').doc(userId).delete();
    return { success: true };
  } catch (error) {
    console.error('Error leaving matchmaking:', error);
    throw new functions.https.HttpsError('internal', 'Failed to leave queue');
  }
});

/**
 * Update match score
 */
exports.updateMatchScore = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { matchId, score } = data;

  try {
    const matchRef = db.collection('matches').doc(matchId);
    await matchRef.update({
      [`scores.${userId}`]: score,
      [`finishedAt.${userId}`]: admin.firestore.FieldValue.serverTimestamp()
    });

    // Check if both players finished
    const matchDoc = await matchRef.get();
    const matchData = matchDoc.data();

    const player1Finished = matchData.scores[matchData.player1] > 0;
    const player2Finished = matchData.scores[matchData.player2] > 0;

    if (player1Finished && player2Finished) {
      // Both finished, determine winner
      const result = await determineWinner(matchData);
      return { finished: true, result: result };
    }

    return { finished: false };
  } catch (error) {
    console.error('Error updating match score:', error);
    throw new functions.https.HttpsError('internal', 'Failed to update score');
  }
});

// ============================================================================
// SHOP & MONETIZATION SYSTEM
// ============================================================================

/**
 * Purchase item with coins
 */
exports.purchaseWithCoins = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { itemId, cost } = data;

  try {
    const userRef = db.collection('users').doc(userId);
    const userDoc = await userRef.get();
    const userData = userDoc.data();

    // Check balance
    if (userData.coins < cost) {
      throw new functions.https.HttpsError('failed-precondition', 'Insufficient coins');
    }

    // Deduct coins
    await userRef.update({
      coins: admin.firestore.FieldValue.increment(-cost)
    });

    // Award item (add to unlocked skins)
    await userRef.update({
      unlockedSkins: admin.firestore.FieldValue.arrayUnion(itemId)
    });

    // Log transaction
    await db.collection('transactions').add({
      userId: userId,
      type: 'coin_purchase',
      itemId: itemId,
      cost: cost,
      timestamp: admin.firestore.FieldValue.serverTimestamp()
    });

    return {
      success: true,
      newBalance: userData.coins - cost,
      item: itemId
    };
  } catch (error) {
    console.error('Error purchasing with coins:', error);
    throw error;
  }
});

/**
 * Verify IAP purchase (Apple/Google)
 */
exports.verifyIAP = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { platform, receipt, productId } = data;

  try {
    // Verify receipt with platform
    let isValid = false;
    if (platform === 'ios') {
      isValid = await verifyAppleReceipt(receipt);
    } else if (platform === 'android') {
      isValid = await verifyGoogleReceipt(receipt);
    }

    if (!isValid) {
      throw new functions.https.HttpsError('invalid-argument', 'Invalid receipt');
    }

    // Award purchase based on productId
    const reward = getProductReward(productId);

    const userRef = db.collection('users').doc(userId);
    await userRef.update({
      coins: admin.firestore.FieldValue.increment(reward.coins || 0),
      gems: admin.firestore.FieldValue.increment(reward.gems || 0)
    });

    // Log transaction
    await db.collection('transactions').add({
      userId: userId,
      type: 'iap',
      platform: platform,
      productId: productId,
      timestamp: admin.firestore.FieldValue.serverTimestamp()
    });

    return {
      success: true,
      reward: reward
    };
  } catch (error) {
    console.error('Error verifying IAP:', error);
    throw error;
  }
});

/**
 * Award currency (for rewards, challenges, etc.)
 */
exports.awardCurrency = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { type, amount, reason } = data;

  try {
    const update = {};
    update[type] = admin.firestore.FieldValue.increment(amount);

    await db.collection('users').doc(userId).update(update);

    // Log
    await db.collection('currency_logs').add({
      userId: userId,
      type: type,
      amount: amount,
      reason: reason,
      timestamp: admin.firestore.FieldValue.serverTimestamp()
    });

    return { success: true };
  } catch (error) {
    console.error('Error awarding currency:', error);
    throw new functions.https.HttpsError('internal', 'Failed to award currency');
  }
});

// ============================================================================
// DAILY CHALLENGES & EVENTS
// ============================================================================

/**
 * Get today's daily challenges
 */
exports.getDailyChallenges = functions.https.onCall(async (data, context) => {
  const today = getTodayKey();

  try {
    const challengeDoc = await db.collection('challenges').doc('daily').collection('days').doc(today).get();

    if (challengeDoc.exists) {
      return { challenges: challengeDoc.data().challenges };
    }

    // Generate new challenges for today
    const challenges = generateDailyChallenges();
    await db.collection('challenges').doc('daily').collection('days').doc(today).set({
      challenges: challenges,
      generatedAt: admin.firestore.FieldValue.serverTimestamp()
    });

    return { challenges: challenges };
  } catch (error) {
    console.error('Error getting daily challenges:', error);
    throw new functions.https.HttpsError('internal', 'Failed to get challenges');
  }
});

/**
 * Update challenge progress
 */
exports.updateChallengeProgress = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { challengeId, value } = data;
  const today = getTodayKey();

  try {
    const progressRef = db.collection('progress').doc(userId).collection('challenges').doc(today);

    const update = {};
    update[challengeId] = value;

    await progressRef.set(update, { merge: true });

    // Check if challenge completed
    const challengeDoc = await db.collection('challenges').doc('daily').collection('days').doc(today).get();
    const challenges = challengeDoc.data().challenges;
    const challenge = challenges.find(c => c.id === challengeId);

    if (challenge && value >= challenge.target) {
      // Award reward
      if (challenge.reward.coins) {
        await db.collection('users').doc(userId).update({
          coins: admin.firestore.FieldValue.increment(challenge.reward.coins)
        });
      }
      if (challenge.reward.gems) {
        await db.collection('users').doc(userId).update({
          gems: admin.firestore.FieldValue.increment(challenge.reward.gems)
        });
      }

      return {
        completed: true,
        reward: challenge.reward
      };
    }

    return { completed: false };
  } catch (error) {
    console.error('Error updating challenge progress:', error);
    throw new functions.https.HttpsError('internal', 'Failed to update progress');
  }
});

// ============================================================================
// ANALYTICS & TRACKING
// ============================================================================

/**
 * Track game event
 */
exports.trackEvent = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { eventName, properties } = data;

  try {
    await db.collection('analytics').doc('events').collection('all').add({
      userId: userId,
      eventName: eventName,
      properties: properties,
      timestamp: admin.firestore.FieldValue.serverTimestamp()
    });

    return { success: true };
  } catch (error) {
    console.error('Error tracking event:', error);
    // Don't throw error for analytics - fail silently
    return { success: false };
  }
});

// ============================================================================
// SOCIAL FEATURES (FRIENDS & CLANS)
// ============================================================================

/**
 * Send friend request
 */
exports.sendFriendRequest = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { friendId } = data;

  if (userId === friendId) {
    throw new functions.https.HttpsError('invalid-argument', 'Cannot add yourself');
  }

  try {
    // Add to sender's friends list (pending)
    await db.collection('friends').doc(userId).collection('list').doc(friendId).set({
      status: 'pending',
      sentAt: admin.firestore.FieldValue.serverTimestamp()
    });

    // Add to receiver's friend requests
    await db.collection('friends').doc(friendId).collection('requests').doc(userId).set({
      status: 'pending',
      receivedAt: admin.firestore.FieldValue.serverTimestamp()
    });

    // TODO: Send push notification to friend

    return { success: true };
  } catch (error) {
    console.error('Error sending friend request:', error);
    throw new functions.https.HttpsError('internal', 'Failed to send request');
  }
});

/**
 * Accept friend request
 */
exports.acceptFriendRequest = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { friendId } = data;

  try {
    // Update both sides to accepted
    await db.collection('friends').doc(userId).collection('list').doc(friendId).set({
      status: 'accepted',
      acceptedAt: admin.firestore.FieldValue.serverTimestamp()
    });

    await db.collection('friends').doc(friendId).collection('list').doc(userId).set({
      status: 'accepted',
      acceptedAt: admin.firestore.FieldValue.serverTimestamp()
    });

    // Remove request
    await db.collection('friends').doc(userId).collection('requests').doc(friendId).delete();

    // Update friend counts
    await db.collection('users').doc(userId).update({
      friendCount: admin.firestore.FieldValue.increment(1)
    });
    await db.collection('users').doc(friendId).update({
      friendCount: admin.firestore.FieldValue.increment(1)
    });

    return { success: true };
  } catch (error) {
    console.error('Error accepting friend request:', error);
    throw new functions.https.HttpsError('internal', 'Failed to accept request');
  }
});

/**
 * Get friends list
 */
exports.getFriends = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;

  try {
    const friendsSnapshot = await db.collection('friends').doc(userId).collection('list')
      .where('status', '==', 'accepted')
      .get();

    const friends = [];
    for (const doc of friendsSnapshot.docs) {
      const friendId = doc.id;
      const friendData = await db.collection('users').doc(friendId).get();
      friends.push({
        userId: friendId,
        ...friendData.data()
      });
    }

    return { friends: friends };
  } catch (error) {
    console.error('Error getting friends:', error);
    throw new functions.https.HttpsError('internal', 'Failed to get friends');
  }
});

/**
 * Create clan
 */
exports.createClan = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { name, description } = data;

  try {
    const clanId = db.collection('clans').doc().id;

    await db.collection('clans').doc(clanId).set({
      id: clanId,
      name: name,
      description: description || '',
      leaderId: userId,
      createdAt: admin.firestore.FieldValue.serverTimestamp(),
      memberCount: 1,
      totalScore: 0
    });

    // Add creator as member
    await db.collection('clans').doc(clanId).collection('members').doc(userId).set({
      role: 'leader',
      joinedAt: admin.firestore.FieldValue.serverTimestamp()
    });

    // Update user
    await db.collection('users').doc(userId).update({
      clanId: clanId
    });

    return { success: true, clanId: clanId };
  } catch (error) {
    console.error('Error creating clan:', error);
    throw new functions.https.HttpsError('internal', 'Failed to create clan');
  }
});

/**
 * Join clan
 */
exports.joinClan = functions.https.onCall(async (data, context) => {
  if (!context.auth) {
    throw new functions.https.HttpsError('unauthenticated', 'User must be authenticated');
  }

  const userId = context.auth.uid;
  const { clanId } = data;

  try {
    const clanDoc = await db.collection('clans').doc(clanId).get();

    if (!clanDoc.exists) {
      throw new functions.https.HttpsError('not-found', 'Clan not found');
    }

    const clanData = clanDoc.data();
    if (clanData.memberCount >= 50) {
      throw new functions.https.HttpsError('failed-precondition', 'Clan is full');
    }

    // Add member
    await db.collection('clans').doc(clanId).collection('members').doc(userId).set({
      role: 'member',
      joinedAt: admin.firestore.FieldValue.serverTimestamp()
    });

    // Update clan stats
    await db.collection('clans').doc(clanId).update({
      memberCount: admin.firestore.FieldValue.increment(1)
    });

    // Update user
    await db.collection('users').doc(userId).update({
      clanId: clanId
    });

    return { success: true };
  } catch (error) {
    console.error('Error joining clan:', error);
    throw error;
  }
});

// ============================================================================
// HELPER FUNCTIONS
// ============================================================================

function getTodayKey() {
  const now = new Date();
  return `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}-${String(now.getDate()).padStart(2, '0')}`;
}

function getWeekKey() {
  const now = new Date();
  const year = now.getFullYear();
  const week = getWeekNumber(now);
  return `${year}-W${String(week).padStart(2, '0')}`;
}

function getWeekNumber(date) {
  const d = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
  const dayNum = d.getUTCDay() || 7;
  d.setUTCDate(d.getUTCDate() + 4 - dayNum);
  const yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
  return Math.ceil((((d - yearStart) / 86400000) + 1) / 7);
}

function validateScore(score, gameData) {
  // Basic validation
  if (score < 0) return false;
  if (score > 1000000) return false; // Arbitrary max

  // Check if score is reasonable given game data
  if (gameData) {
    const maxPossibleScore = (gameData.ballsUsed || 100) * 4096 * 2; // Max ball value * 2x for combos
    if (score > maxPossibleScore) return false;

    // Check game duration (can't get 100k in 5 seconds)
    if (gameData.duration && gameData.duration < 10 && score > 10000) return false;
  }

  return true;
}

async function calculateRank(userId, score, type) {
  let collection;
  if (type === 'allTime') {
    collection = db.collection('leaderboards').doc('allTime').collection('scores');
  } else if (type === 'daily') {
    const today = getTodayKey();
    collection = db.collection('leaderboards').doc('daily').collection(today);
  } else if (type === 'weekly') {
    const week = getWeekKey();
    collection = db.collection('leaderboards').doc('weekly').collection(week);
  }

  const higherScores = await collection
    .where('score', '>', score)
    .get();

  return higherScores.size + 1;
}

async function findMatch(userId, skillRating) {
  const queueSnapshot = await db.collection('matchmaking').doc('queue').collection('players')
    .where('skillRating', '>=', skillRating - 200)
    .where('skillRating', '<=', skillRating + 200)
    .where('status', '==', 'searching')
    .limit(10)
    .get();

  let opponent = null;
  queueSnapshot.forEach(doc => {
    if (doc.id !== userId && !opponent) {
      opponent = { id: doc.id, ...doc.data() };
    }
  });

  if (opponent) {
    // Create match
    const matchId = db.collection('matches').doc().id;
    const matchData = {
      matchId: matchId,
      player1: userId,
      player2: opponent.id,
      status: 'active',
      startTime: admin.firestore.FieldValue.serverTimestamp(),
      boardSeed: Math.random(),
      scores: {
        [userId]: 0,
        [opponent.id]: 0
      },
      finishedAt: {}
    };

    await db.collection('matches').doc(matchId).set(matchData);

    // Remove from queue
    await db.collection('matchmaking').doc('queue').collection('players').doc(userId).delete();
    await db.collection('matchmaking').doc('queue').collection('players').doc(opponent.id).delete();

    return matchData;
  }

  return null;
}

async function determineWinner(matchData) {
  const p1Score = matchData.scores[matchData.player1];
  const p2Score = matchData.scores[matchData.player2];

  const winner = p1Score > p2Score ? matchData.player1 : matchData.player2;
  const loser = p1Score > p2Score ? matchData.player2 : matchData.player1;

  const result = {
    winner: winner,
    loser: loser,
    winnerScore: Math.max(p1Score, p2Score),
    loserScore: Math.min(p1Score, p2Score)
  };

  // Award prizes
  await db.collection('users').doc(winner).update({
    coins: admin.firestore.FieldValue.increment(200)
  });
  await db.collection('users').doc(loser).update({
    coins: admin.firestore.FieldValue.increment(50)
  });

  // Save result
  await db.collection('matches').doc(matchData.matchId).update({
    result: result,
    status: 'completed',
    completedAt: admin.firestore.FieldValue.serverTimestamp()
  });

  return result;
}

function generateDailyChallenges() {
  const challengePool = [
    { id: 'merges', type: 'count', target: 10, reward: { coins: 500 }, description: 'Complete 10 merges in one game' },
    { id: 'combo', type: 'achievement', target: 5, reward: { gems: 1 }, description: 'Achieve a 5x combo' },
    { id: 'high_score', type: 'threshold', target: 10000, reward: { coins: 1000 }, description: 'Score 10,000 points in one game' },
    { id: 'powerups', type: 'count', target: 5, reward: { coins: 300 }, description: 'Use 5 power-ups' },
    { id: 'chain', type: 'achievement', target: 7, reward: { gems: 3 }, description: 'Create a 7+ merge chain' },
    { id: 'perfect_shots', type: 'count', target: 10, reward: { coins: 400 }, description: 'Land 10 perfect shots' },
    { id: 'reach_128', type: 'achievement', target: 128, reward: { gems: 2 }, description: 'Create a 128 ball' },
    { id: 'games_played', type: 'count', target: 5, reward: { coins: 300 }, description: 'Play 5 games' }
  ];

  // Shuffle and pick 3
  const shuffled = challengePool.sort(() => 0.5 - Math.random());
  return shuffled.slice(0, 3);
}

function getProductReward(productId) {
  const products = {
    'coins_small': { coins: 1000 },
    'coins_medium': { coins: 3500 },
    'coins_large': { coins: 6500 },
    'coins_mega': { coins: 15000 },
    'gems_small': { gems: 5 },
    'gems_medium': { gems: 15 },
    'gems_large': { gems: 50 }
  };

  return products[productId] || { coins: 0, gems: 0 };
}

async function verifyAppleReceipt(receipt) {
  // TODO: Implement Apple App Store receipt verification
  // Use sandbox URL for testing: https://sandbox.itunes.apple.com/verifyReceipt
  // Use production URL for live: https://buy.itunes.apple.com/verifyReceipt
  return true; // Placeholder
}

async function verifyGoogleReceipt(receipt) {
  // TODO: Implement Google Play receipt verification
  // Use Google Play Developer API
  return true; // Placeholder
}
