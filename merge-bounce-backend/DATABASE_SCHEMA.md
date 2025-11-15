# Merge Bounce - Complete Firebase Database Schema

## Firestore Collections Structure

### 1. users/{userId}
**Purpose:** Store user profiles and game data

```javascript
{
  uid: string,                    // Firebase Auth UID
  email: string,                  // User email
  displayName: string,            // Player display name
  photoURL: string | null,        // Profile picture URL
  createdAt: timestamp,           // Account creation time
  lastLogin: timestamp,           // Last login time

  // Game Stats
  level: number,                  // Player level (1-100)
  experience: number,             // Total XP earned
  totalGamesPlayed: number,       // Lifetime games played
  highScore: number,              // All-time high score
  totalMerges: number,            // Lifetime merges completed
  highestCombo: number,           // Best combo achieved
  extremeFeverCount: number,      // Times triggered Extreme Fever

  // Currency
  coins: number,                  // Soft currency
  gems: number,                   // Hard currency (premium)

  // Progression
  unlockedSkins: [string],        // Array of unlocked skin IDs
  upgrades: {
    ballTrail: number,            // Upgrade level (0-5)
    comboWindow: number,          // Upgrade level (0-5)
    powerupFrequency: number,     // Upgrade level (0-5)
    startingBall: number          // Upgrade level (0-5)
  },

  // Premium
  isPremium: boolean,             // Premium subscription status
  premiumExpiryDate: timestamp | null,

  // Social
  clanId: string | null,          // Current clan membership
  friendCount: number,            // Number of friends

  // Misc
  tutorialCompleted: boolean,     // Tutorial completion flag
  dailyLoginStreak: number,       // Current login streak
  lastDailyLogin: timestamp       // Last daily login date
}
```

---

### 2. saves/{userId}
**Purpose:** Cloud save data (mirrors user data for sync)

```javascript
{
  coins: number,
  gems: number,
  highScore: number,
  level: number,
  experience: number,
  unlockedSkins: [string],
  upgrades: object,
  stats: {
    totalGamesPlayed: number,
    totalMerges: number,
    highestCombo: number,
    extremeFeverCount: number
  },
  lastSaved: timestamp
}
```

---

### 3. leaderboards/allTime/scores/{userId}
**Purpose:** All-time high score leaderboard

```javascript
{
  userId: string,
  score: number,
  displayName: string,
  level: number,
  timestamp: timestamp
}
```

### 4. leaderboards/daily/{YYYY-MM-DD}/{userId}
**Purpose:** Daily leaderboard (resets each day)

```javascript
{
  userId: string,
  score: number,
  displayName: string,
  timestamp: timestamp
}
```

### 5. leaderboards/weekly/{YYYY-WWW}/{userId}
**Purpose:** Weekly leaderboard (resets each week)

```javascript
{
  userId: string,
  score: number,
  displayName: string,
  timestamp: timestamp
}
```

---

### 6. matchmaking/queue/players/{userId}
**Purpose:** Matchmaking queue for PvP battles

```javascript
{
  userId: string,
  skillRating: number,            // ELO-like rating for matchmaking
  joinedAt: timestamp,
  status: string                  // 'searching' | 'matched'
}
```

---

### 7. matches/{matchId}
**Purpose:** Active and completed PvP matches

```javascript
{
  matchId: string,
  player1: string,                // User ID
  player2: string,                // User ID
  status: string,                 // 'active' | 'completed'
  startTime: timestamp,
  boardSeed: number,              // Random seed for identical boards
  scores: {
    [player1]: number,
    [player2]: number
  },
  finishedAt: {
    [player1]: timestamp,
    [player2]: timestamp
  },
  result: {                       // Set when both players finish
    winner: string,
    loser: string,
    winnerScore: number,
    loserScore: number
  }
}
```

---

### 8. transactions/{transactionId}
**Purpose:** Purchase transaction log

```javascript
{
  userId: string,
  type: string,                   // 'iap' | 'coin_purchase'
  platform: string,               // 'ios' | 'android' (for IAP)
  productId: string,
  itemId: string,                 // For coin purchases
  cost: number,                   // For coin purchases
  timestamp: timestamp
}
```

---

### 9. challenges/daily/days/{YYYY-MM-DD}
**Purpose:** Daily challenge definitions

```javascript
{
  challenges: [
    {
      id: string,                 // 'merges', 'combo', 'high_score', etc.
      type: string,               // 'count' | 'achievement' | 'threshold'
      target: number,             // Goal to reach
      reward: {
        coins: number,
        gems: number
      },
      description: string
    }
  ],
  generatedAt: timestamp
}
```

---

### 10. progress/{userId}/challenges/{YYYY-MM-DD}
**Purpose:** User's progress on daily challenges

```javascript
{
  [challengeId]: number          // Current progress value
}
```

---

### 11. friends/{userId}/list/{friendId}
**Purpose:** User's friends list

```javascript
{
  status: string,                // 'pending' | 'accepted'
  sentAt: timestamp,             // When request was sent
  acceptedAt: timestamp          // When accepted
}
```

### 12. friends/{userId}/requests/{friendId}
**Purpose:** Incoming friend requests

```javascript
{
  status: string,                // 'pending'
  receivedAt: timestamp
}
```

---

### 13. clans/{clanId}
**Purpose:** Clan/guild information

```javascript
{
  id: string,
  name: string,
  description: string,
  leaderId: string,              // User ID of clan leader
  createdAt: timestamp,
  memberCount: number,
  totalScore: number             // Combined score of all members
}
```

### 14. clans/{clanId}/members/{userId}
**Purpose:** Clan membership

```javascript
{
  role: string,                  // 'leader' | 'member'
  joinedAt: timestamp
}
```

---

### 15. analytics/events/all/{eventId}
**Purpose:** Analytics event tracking

```javascript
{
  userId: string,
  eventName: string,             // 'game_start', 'merge_completed', etc.
  properties: object,            // Event-specific data
  timestamp: timestamp
}
```

---

### 16. suspicious_scores/{scoreId}
**Purpose:** Flagged suspicious scores for review

```javascript
{
  userId: string,
  score: number,
  gameData: object,
  timestamp: timestamp
}
```

---

### 17. currency_logs/{logId}
**Purpose:** Currency award/spend tracking

```javascript
{
  userId: string,
  type: string,                  // 'coins' | 'gems'
  amount: number,
  reason: string,                // 'daily_challenge', 'purchase', etc.
  timestamp: timestamp
}
```

---

## Firestore Indexes

Required composite indexes for optimal query performance:

```json
{
  "indexes": [
    {
      "collectionGroup": "scores",
      "queryScope": "COLLECTION",
      "fields": [
        { "fieldPath": "score", "order": "DESCENDING" }
      ]
    },
    {
      "collectionGroup": "players",
      "queryScope": "COLLECTION",
      "fields": [
        { "fieldPath": "skillRating", "order": "ASCENDING" },
        { "fieldPath": "status", "order": "ASCENDING" }
      ]
    }
  ]
}
```

---

## Data Access Patterns

### Read Operations:
- **Get User Profile:** `users/{userId}` (1 read)
- **Load Progress:** `saves/{userId}` (1 read)
- **Get Top 100 Leaderboard:** `leaderboards/allTime/scores` ordered by score desc (100 reads)
- **Get Player Rank:** Count scores > player's score (1 read + aggregation)
- **Get Friends List:** `friends/{userId}/list` where status == 'accepted' (N reads)
- **Get Daily Challenges:** `challenges/daily/days/{today}` (1 read)

### Write Operations:
- **Save Progress:** `saves/{userId}` + `users/{userId}` (2 writes)
- **Submit Score:** `leaderboards/allTime/scores/{userId}` + daily + weekly (3 writes)
- **Update Challenge Progress:** `progress/{userId}/challenges/{today}` (1 write)
- **Accept Friend:** `friends/{userId}/list/{friendId}` + `friends/{friendId}/list/{userId}` (2 writes)

---

## Cost Estimation (Firebase Free Tier)

### Free Tier Limits:
- **Firestore:** 50,000 reads/day, 20,000 writes/day, 20,000 deletes/day
- **Authentication:** 10,000 verifications/month
- **Cloud Functions:** 2,000,000 invocations/month, 400,000 GB-seconds/month
- **Storage:** 5 GB
- **Hosting:** 10 GB transfer/month

### Expected Usage (1,000 Daily Active Users):
- **Reads:** ~15 per user/session = 15,000/day ✅ Within free tier
- **Writes:** ~5 per user/session = 5,000/day ✅ Within free tier
- **Cloud Functions:** ~20 per user/session = 20,000/day ✅ Within free tier

**Conclusion:** Free tier supports 1,000-5,000 DAU easily. Paid tier needed at ~10,000+ DAU.

---

## Security Considerations

1. **All writes go through Cloud Functions** for validation
2. **Leaderboards are read-only** from client (prevents cheating)
3. **Transactions logged** for audit trail
4. **Suspicious scores flagged** for manual review
5. **User data isolated** by security rules
6. **Friends/social features** require authentication

---

## Backup Strategy

1. **Automated daily backups** via Firebase console
2. **Export to Cloud Storage** weekly
3. **Retention:** 30 days of backups
4. **Disaster recovery:** Restore from backup within 4 hours

---

## Scalability

### Current Schema Supports:
- ✅ 100,000+ users
- ✅ 1,000+ concurrent players
- ✅ 10,000+ daily matches
- ✅ Real-time leaderboards
- ✅ Sub-second query performance

### When to Scale:
- **10,000+ DAU:** Move to Firestore in Datastore mode
- **100,000+ DAU:** Add caching layer (Redis)
- **1M+ DAU:** Shard leaderboards by region
