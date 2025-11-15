using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Functions;
using Firebase.Firestore;

namespace MergeBounce
{
    /// <summary>
    /// Handles all Firebase backend communication
    /// Connects Unity game to cloud functions, auth, and Firestore
    /// </summary>
    public class BackendConnector : MonoBehaviour
    {
        public static BackendConnector Instance { get; private set; }

        [Header("Firebase Status")]
        public bool isInitialized = false;
        public bool isAuthenticated = false;

        // Firebase references
        private FirebaseApp app;
        private FirebaseAuth auth;
        private FirebaseFunctions functions;
        private FirebaseFirestore firestore;

        // User data
        private FirebaseUser currentUser;
        public string userId { get; private set; }

        // Events
        public System.Action<bool> OnAuthenticationChanged;
        public System.Action<UserProfile> OnProfileLoaded;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            InitializeFirebase();
        }

        /// <summary>
        /// Initialize Firebase SDK
        /// </summary>
        async void InitializeFirebase()
        {
            Debug.Log("Initializing Firebase...");

            try
            {
                var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

                if (dependencyStatus == DependencyStatus.Available)
                {
                    app = FirebaseApp.DefaultInstance;
                    auth = FirebaseAuth.DefaultInstance;
                    functions = FirebaseFunctions.DefaultInstance;
                    firestore = FirebaseFirestore.DefaultInstance;

                    isInitialized = true;
                    Debug.Log("Firebase initialized successfully!");

                    // Check if user is already signed in
                    if (auth.CurrentUser != null)
                    {
                        OnUserSignedIn(auth.CurrentUser);
                    }
                }
                else
                {
                    Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Firebase initialization error: {e.Message}");
            }
        }

        // ============================================================================
        // AUTHENTICATION
        // ============================================================================

        /// <summary>
        /// Sign up with email and password
        /// </summary>
        public async Task<bool> SignUpWithEmail(string email, string password, string displayName)
        {
            if (!isInitialized)
            {
                Debug.LogError("Firebase not initialized");
                return false;
            }

            try
            {
                var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
                currentUser = result.User;

                // Set display name
                var profile = new Firebase.Auth.UserProfile { DisplayName = displayName };
                await currentUser.UpdateUserProfileAsync(profile);

                OnUserSignedIn(currentUser);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Sign up error: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Sign in with email and password
        /// </summary>
        public async Task<bool> SignInWithEmail(string email, string password)
        {
            if (!isInitialized)
            {
                Debug.LogError("Firebase not initialized");
                return false;
            }

            try
            {
                var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
                currentUser = result.User;

                OnUserSignedIn(currentUser);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Sign in error: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Sign out
        /// </summary>
        public void SignOut()
        {
            auth.SignOut();
            currentUser = null;
            userId = null;
            isAuthenticated = false;
            OnAuthenticationChanged?.Invoke(false);
        }

        /// <summary>
        /// Called when user signs in
        /// </summary>
        void OnUserSignedIn(FirebaseUser user)
        {
            currentUser = user;
            userId = user.UserId;
            isAuthenticated = true;

            Debug.Log($"User signed in: {user.DisplayName} ({user.UserId})");

            OnAuthenticationChanged?.Invoke(true);

            // Update last login
            UpdateLastLogin();

            // Load user profile
            LoadUserProfile();
        }

        // ============================================================================
        // CLOUD FUNCTIONS
        // ============================================================================

        /// <summary>
        /// Call a cloud function
        /// </summary>
        async Task<Dictionary<string, object>> CallFunction(string functionName, Dictionary<string, object> data = null)
        {
            if (!isInitialized)
            {
                Debug.LogError("Firebase not initialized");
                return null;
            }

            try
            {
                var function = functions.GetHttpsCallable(functionName);
                var result = await function.CallAsync(data);

                return result.Data as Dictionary<string, object>;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Cloud function error ({functionName}): {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Update last login (tracks login streaks)
        /// </summary>
        async void UpdateLastLogin()
        {
            var result = await CallFunction("updateLastLogin");
            if (result != null && result.ContainsKey("streak"))
            {
                int streak = System.Convert.ToInt32(result["streak"]);
                Debug.Log($"Login streak: {streak} days");
            }
        }

        // ============================================================================
        // CLOUD SAVE SYSTEM
        // ============================================================================

        /// <summary>
        /// Save progress to cloud
        /// </summary>
        public async Task<bool> SaveProgress(GameSaveData saveData)
        {
            var data = new Dictionary<string, object>
            {
                { "coins", saveData.coins },
                { "gems", saveData.gems },
                { "highScore", saveData.highScore },
                { "level", saveData.level },
                { "experience", saveData.experience },
                { "unlockedSkins", saveData.unlockedSkins },
                { "upgrades", saveData.upgrades },
                { "totalGamesPlayed", saveData.totalGamesPlayed },
                { "totalMerges", saveData.totalMerges },
                { "highestCombo", saveData.highestCombo },
                { "extremeFeverCount", saveData.extremeFeverCount }
            };

            var result = await CallFunction("saveProgress", data);
            return result != null && result.ContainsKey("success");
        }

        /// <summary>
        /// Load progress from cloud
        /// </summary>
        public async Task<GameSaveData> LoadProgress()
        {
            var result = await CallFunction("loadProgress");

            if (result == null) return null;

            var saveData = new GameSaveData
            {
                coins = GetIntValue(result, "coins"),
                gems = GetIntValue(result, "gems"),
                highScore = GetIntValue(result, "highScore"),
                level = GetIntValue(result, "level"),
                experience = GetIntValue(result, "experience"),
                totalGamesPlayed = GetIntValue(result, "totalGamesPlayed"),
                totalMerges = GetIntValue(result, "totalMerges"),
                highestCombo = GetIntValue(result, "highestCombo"),
                extremeFeverCount = GetIntValue(result, "extremeFeverCount")
            };

            return saveData;
        }

        // ============================================================================
        // LEADERBOARDS
        // ============================================================================

        /// <summary>
        /// Submit score to leaderboard
        /// </summary>
        public async Task<IEnumerator> SubmitScore(GameStats stats)
        {
            var gameData = new Dictionary<string, object>
            {
                { "ballsUsed", stats.ballsUsed },
                { "duration", stats.gameTime },
                { "previousHighScore", PlayerData.HighScore }
            };

            var data = new Dictionary<string, object>
            {
                { "score", stats.finalScore },
                { "gameData", gameData }
            };

            var result = await CallFunction("submitScore", data);

            if (result != null && result.ContainsKey("rank"))
            {
                int rank = System.Convert.ToInt32(result["rank"]);
                Debug.Log($"Score submitted! Rank: #{rank}");

                // Show rank to player
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowRank(rank);
                }
            }

            yield return null;
        }

        /// <summary>
        /// Get leaderboard
        /// </summary>
        public async Task<List<LeaderboardEntry>> GetLeaderboard(string type = "allTime", int limit = 100)
        {
            var data = new Dictionary<string, object>
            {
                { "type", type },
                { "limit", limit }
            };

            var result = await CallFunction("getLeaderboard", data);

            if (result == null || !result.ContainsKey("scores")) return null;

            var scoresList = result["scores"] as List<object>;
            var entries = new List<LeaderboardEntry>();

            foreach (var scoreObj in scoresList)
            {
                var scoreDict = scoreObj as Dictionary<string, object>;
                entries.Add(new LeaderboardEntry
                {
                    rank = GetIntValue(scoreDict, "rank"),
                    userId = GetStringValue(scoreDict, "userId"),
                    displayName = GetStringValue(scoreDict, "displayName"),
                    score = GetIntValue(scoreDict, "score")
                });
            }

            return entries;
        }

        /// <summary>
        /// Get player's rank
        /// </summary>
        public async Task<int> GetPlayerRank(string type = "allTime")
        {
            var data = new Dictionary<string, object>
            {
                { "type", type }
            };

            var result = await CallFunction("getPlayerRank", data);

            if (result != null && result.ContainsKey("rank"))
            {
                return System.Convert.ToInt32(result["rank"]);
            }

            return -1; // Not ranked
        }

        // ============================================================================
        // SHOP & PURCHASES
        // ============================================================================

        /// <summary>
        /// Purchase item with coins
        /// </summary>
        public async Task<bool> PurchaseWithCoins(string itemId, int cost)
        {
            var data = new Dictionary<string, object>
            {
                { "itemId", itemId },
                { "cost", cost }
            };

            var result = await CallFunction("purchaseWithCoins", data);
            return result != null && result.ContainsKey("success");
        }

        /// <summary>
        /// Verify in-app purchase
        /// </summary>
        public async Task<bool> VerifyIAP(string platform, string receipt, string productId)
        {
            var data = new Dictionary<string, object>
            {
                { "platform", platform },
                { "receipt", receipt },
                { "productId", productId }
            };

            var result = await CallFunction("verifyIAP", data);
            return result != null && result.ContainsKey("success");
        }

        // ============================================================================
        // DAILY CHALLENGES
        // ============================================================================

        /// <summary>
        /// Get today's challenges
        /// </summary>
        public async Task<List<DailyChallenge>> GetDailyChallenges()
        {
            var result = await CallFunction("getDailyChallenges");

            if (result == null || !result.ContainsKey("challenges")) return null;

            var challengesList = result["challenges"] as List<object>;
            var challenges = new List<DailyChallenge>();

            foreach (var challengeObj in challengesList)
            {
                var challengeDict = challengeObj as Dictionary<string, object>;
                // Parse challenge data...
                // TODO: Implement parsing
            }

            return challenges;
        }

        /// <summary>
        /// Update challenge progress
        /// </summary>
        public async Task UpdateChallengeProgress(string challengeId, int value)
        {
            var data = new Dictionary<string, object>
            {
                { "challengeId", challengeId },
                { "value", value }
            };

            await CallFunction("updateChallengeProgress", data);
        }

        // ============================================================================
        // ANALYTICS
        // ============================================================================

        /// <summary>
        /// Track game event
        /// </summary>
        public async void TrackEvent(string eventName, Dictionary<string, object> properties)
        {
            var data = new Dictionary<string, object>
            {
                { "eventName", eventName },
                { "properties", properties }
            };

            await CallFunction("trackEvent", data);
        }

        // ============================================================================
        // SOCIAL FEATURES
        // ============================================================================

        /// <summary>
        /// Send friend request
        /// </summary>
        public async Task<bool> SendFriendRequest(string friendId)
        {
            var data = new Dictionary<string, object>
            {
                { "friendId", friendId }
            };

            var result = await CallFunction("sendFriendRequest", data);
            return result != null && result.ContainsKey("success");
        }

        /// <summary>
        /// Get friends list
        /// </summary>
        public async Task<List<Friend>> GetFriends()
        {
            var result = await CallFunction("getFriends");

            if (result == null || !result.ContainsKey("friends")) return null;

            // TODO: Parse friends list
            return new List<Friend>();
        }

        // ============================================================================
        // HELPER METHODS
        // ============================================================================

        async void LoadUserProfile()
        {
            var saveData = await LoadProgress();

            if (saveData != null)
            {
                // Apply loaded data to game
                PlayerData.Coins = saveData.coins;
                PlayerData.Gems = saveData.gems;
                PlayerData.HighScore = saveData.highScore;
                PlayerData.Level = saveData.level;

                Debug.Log($"Profile loaded: Level {saveData.level}, {saveData.coins} coins");
            }
        }

        int GetIntValue(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return System.Convert.ToInt32(dict[key]);
            }
            return 0;
        }

        string GetStringValue(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key].ToString();
            }
            return "";
        }
    }

    // ============================================================================
    // DATA STRUCTURES
    // ============================================================================

    [System.Serializable]
    public class GameSaveData
    {
        public int coins;
        public int gems;
        public int highScore;
        public int level;
        public int experience;
        public List<string> unlockedSkins;
        public Dictionary<string, int> upgrades;
        public int totalGamesPlayed;
        public int totalMerges;
        public int highestCombo;
        public int extremeFeverCount;
    }

    [System.Serializable]
    public class UserProfile
    {
        public string userId;
        public string displayName;
        public int level;
        public int coins;
        public int gems;
    }

    [System.Serializable]
    public class LeaderboardEntry
    {
        public int rank;
        public string userId;
        public string displayName;
        public int score;
    }

    [System.Serializable]
    public class DailyChallenge
    {
        public string id;
        public string description;
        public int target;
        public int currentProgress;
        public ChallengeReward reward;
    }

    [System.Serializable]
    public class ChallengeReward
    {
        public int coins;
        public int gems;
    }

    [System.Serializable]
    public class Friend
    {
        public string userId;
        public string displayName;
        public int level;
        public int highScore;
    }

    // Static player data (accessible from anywhere)
    public static class PlayerData
    {
        public static string UserId { get; set; }
        public static int Coins { get; set; }
        public static int Gems { get; set; }
        public static int HighScore { get; set; }
        public static int Level { get; set; }
        public static List<string> UnlockedSkins { get; set; } = new List<string>();
    }
}
