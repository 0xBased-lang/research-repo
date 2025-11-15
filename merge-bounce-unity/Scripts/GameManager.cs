using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MergeBounce
{
    /// <summary>
    /// Main game manager - controls overall game flow and state
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        public GameState currentState = GameState.Menu;
        public int currentScore = 0;
        public int ballsUsed = 0;
        public float gameTime = 0f;
        public bool gameOver = false;

        [Header("References")]
        public BallController currentBall;
        public MergeSystem mergeSystem;
        public ComboManager comboManager;
        public PowerUpSystem powerUpSystem;
        public ScoreManager scoreManager;
        public UIManager uiManager;
        public AudioManager audioManager;
        public BackendConnector backendConnector;

        [Header("Game Settings")]
        public int powerUpInterval = 5; // Every 5 balls
        public float maxGameTime = 300f; // 5 minutes max per game
        public int startingCoins = 100;

        // Events
        public System.Action<int> OnScoreChanged;
        public System.Action<int> OnBallLaunched;
        public System.Action<int, int> OnMergeCompleted; // fromValue, toValue
        public System.Action<int> OnComboAchieved; // comboLevel
        public System.Action OnExtremeFever;
        public System.Action OnGameOver;

        // Statistics
        private int mergeCount = 0;
        private int highestCombo = 0;
        private int extremeFeverCount = 0;
        private float gameStartTime;

        void Awake()
        {
            // Singleton pattern
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

            InitializeGame();
        }

        void Update()
        {
            if (currentState == GameState.Playing)
            {
                gameTime = Time.time - gameStartTime;

                // Check max time
                if (gameTime >= maxGameTime)
                {
                    EndGame();
                }
            }
        }

        void InitializeGame()
        {
            // Get references
            mergeSystem = GetComponent<MergeSystem>();
            comboManager = GetComponent<ComboManager>();
            powerUpSystem = GetComponent<PowerUpSystem>();
            scoreManager = GetComponent<ScoreManager>();
            uiManager = FindObjectOfType<UIManager>();
            audioManager = FindObjectOfType<AudioManager>();
            backendConnector = GetComponent<BackendConnector>();

            // Subscribe to events
            if (mergeSystem != null)
            {
                mergeSystem.OnMerge += HandleMerge;
            }

            if (comboManager != null)
            {
                comboManager.OnComboChanged += HandleCombo;
                comboManager.OnExtremeFever += HandleExtremeFever;
            }
        }

        public void StartGame()
        {
            Debug.Log("Starting new game...");

            currentState = GameState.Playing;
            currentScore = 0;
            ballsUsed = 0;
            gameTime = 0f;
            gameOver = false;
            mergeCount = 0;
            highestCombo = 0;
            extremeFeverCount = 0;
            gameStartTime = Time.time;

            // Reset systems
            if (comboManager != null) comboManager.ResetCombo();
            if (scoreManager != null) scoreManager.ResetScore();

            // Spawn first ball
            SpawnBall();

            // Update UI
            if (uiManager != null)
            {
                uiManager.ShowGameUI();
                uiManager.UpdateScore(0);
            }

            // Track analytics
            if (backendConnector != null)
            {
                backendConnector.TrackEvent("game_start", new Dictionary<string, object>
                {
                    { "timestamp", System.DateTime.Now.ToString() }
                });
            }
        }

        public void EndGame()
        {
            if (gameOver) return; // Already ended

            Debug.Log($"Game Over! Final Score: {currentScore}");

            currentState = GameState.GameOver;
            gameOver = true;

            // Calculate final stats
            GameStats stats = new GameStats
            {
                finalScore = currentScore,
                ballsUsed = ballsUsed,
                mergeCount = mergeCount,
                highestCombo = highestCombo,
                extremeFeverCount = extremeFeverCount,
                gameTime = gameTime
            };

            // Submit score to backend
            if (backendConnector != null)
            {
                StartCoroutine(backendConnector.SubmitScore(stats));
            }

            // Track analytics
            if (backendConnector != null)
            {
                backendConnector.TrackEvent("game_end", new Dictionary<string, object>
                {
                    { "score", currentScore },
                    { "balls_used", ballsUsed },
                    { "merges", mergeCount },
                    { "duration", gameTime }
                });
            }

            // Show game over UI
            if (uiManager != null)
            {
                uiManager.ShowGameOver(stats);
            }

            OnGameOver?.Invoke();
        }

        public void SpawnBall()
        {
            ballsUsed++;

            // Determine ball value
            int ballValue = GetNextBallValue();

            // Check if should spawn power-up ball
            bool isPowerUp = (ballsUsed % powerUpInterval == 0);

            if (isPowerUp && powerUpSystem != null)
            {
                powerUpSystem.SpawnPowerUpBall();
            }
            else
            {
                // Spawn normal ball
                SpawnNormalBall(ballValue);
            }

            OnBallLaunched?.Invoke(ballsUsed);

            // Update UI
            if (uiManager != null)
            {
                uiManager.UpdateBallCount(ballsUsed);
            }
        }

        void SpawnNormalBall(int value)
        {
            // TODO: Instantiate ball prefab at spawn position
            // This will be done in Unity Editor
            Debug.Log($"Spawning ball with value: {value}");

            // For now, just create a placeholder
            // In Unity, you'll do: Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }

        int GetNextBallValue()
        {
            // Starting values: 2, 4, or 8 (based on upgrades)
            int[] possibleValues = { 2, 4, 4, 8 }; // Weighted toward 4
            return possibleValues[Random.Range(0, possibleValues.Length)];
        }

        void HandleMerge(int fromValue, int toValue)
        {
            mergeCount++;

            Debug.Log($"Merge: {fromValue} + {fromValue} = {toValue}");

            // Add score
            AddScore(toValue);

            // Notify combo system
            if (comboManager != null)
            {
                comboManager.OnMerge();
            }

            // Track for challenges
            if (backendConnector != null)
            {
                backendConnector.TrackEvent("merge_completed", new Dictionary<string, object>
                {
                    { "from", fromValue },
                    { "to", toValue },
                    { "merge_count", mergeCount }
                });
            }

            OnMergeCompleted?.Invoke(fromValue, toValue);
        }

        void HandleCombo(int comboLevel)
        {
            Debug.Log($"Combo Level: {comboLevel}x");

            if (comboLevel > highestCombo)
            {
                highestCombo = comboLevel;
            }

            // Update score multiplier
            if (scoreManager != null)
            {
                scoreManager.SetMultiplier(comboLevel);
            }

            OnComboAchieved?.Invoke(comboLevel);
        }

        void HandleExtremeFever()
        {
            extremeFeverCount++;

            Debug.Log("EXTREME FEVER MODE!");

            // Play special music/effects
            if (audioManager != null)
            {
                audioManager.PlayExtremeFeverMusic();
            }

            // Show extreme fever UI
            if (uiManager != null)
            {
                uiManager.ShowExtremeFever();
            }

            OnExtremeFever?.Invoke();
        }

        public void AddScore(int points)
        {
            if (scoreManager != null)
            {
                currentScore = scoreManager.AddScore(points);
            }
            else
            {
                currentScore += points;
            }

            if (uiManager != null)
            {
                uiManager.UpdateScore(currentScore);
            }

            OnScoreChanged?.Invoke(currentScore);
        }

        public void ReturnToMenu()
        {
            currentState = GameState.Menu;

            if (uiManager != null)
            {
                uiManager.ShowMainMenu();
            }
        }

        public void PauseGame()
        {
            if (currentState == GameState.Playing)
            {
                currentState = GameState.Paused;
                Time.timeScale = 0f;

                if (uiManager != null)
                {
                    uiManager.ShowPauseMenu();
                }
            }
        }

        public void ResumeGame()
        {
            if (currentState == GameState.Paused)
            {
                currentState = GameState.Playing;
                Time.timeScale = 1f;

                if (uiManager != null)
                {
                    uiManager.HidePauseMenu();
                }
            }
        }

        public GameStats GetCurrentGameStats()
        {
            return new GameStats
            {
                finalScore = currentScore,
                ballsUsed = ballsUsed,
                mergeCount = mergeCount,
                highestCombo = highestCombo,
                extremeFeverCount = extremeFeverCount,
                gameTime = gameTime
            };
        }
    }

    // Enums and Data Structures
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver
    }

    [System.Serializable]
    public class GameStats
    {
        public int finalScore;
        public int ballsUsed;
        public int mergeCount;
        public int highestCombo;
        public int extremeFeverCount;
        public float gameTime;
    }
}
