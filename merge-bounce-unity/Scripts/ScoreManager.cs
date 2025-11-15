using UnityEngine;

namespace MergeBounce
{
    /// <summary>
    /// Handles score calculation, multipliers, and score events
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        [Header("Score Settings")]
        public int baseScorePerMerge = 10;
        public float[] tierMultipliers = { 1f, 1.2f, 1.5f, 2f, 3f, 5f, 8f, 12f, 20f, 30f, 50f, 100f };

        // State
        private int currentScore = 0;
        private float currentMultiplier = 1f;
        private int highScore = 0;

        // Events
        public System.Action<int> OnScoreChanged;
        public System.Action<int> OnNewHighScore;

        // References
        private ComboManager comboManager;

        void Start()
        {
            comboManager = GetComponent<ComboManager>();

            // Load high score from PlayerPrefs
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        /// <summary>
        /// Add score with automatic multiplier from combo system
        /// </summary>
        public int AddScore(int basePoints)
        {
            // Get combo multiplier
            float comboMultiplier = comboManager != null ? comboManager.GetMultiplier() : 1f;

            // Calculate final score
            int finalPoints = Mathf.RoundToInt(basePoints * comboMultiplier * currentMultiplier);

            // Add to total
            currentScore += finalPoints;

            // Check for new high score
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();

                OnNewHighScore?.Invoke(highScore);
            }

            OnScoreChanged?.Invoke(currentScore);

            Debug.Log($"Score +{finalPoints} (base: {basePoints}, combo: {comboMultiplier}x, multiplier: {currentMultiplier}x) = {currentScore}");

            return currentScore;
        }

        /// <summary>
        /// Calculate score for a merge based on ball value
        /// </summary>
        public int CalculateMergeScore(int ballValue)
        {
            // Higher value balls = more points
            // 2 = 2 points, 4 = 4 points, 8 = 8 points, etc.
            return ballValue;
        }

        /// <summary>
        /// Set temporary multiplier (for power-ups)
        /// </summary>
        public void SetMultiplier(float multiplier)
        {
            currentMultiplier = multiplier;
        }

        /// <summary>
        /// Apply temporary score boost (for power-ups)
        /// </summary>
        public void ApplyScoreBoost(float boostMultiplier, float duration)
        {
            StartCoroutine(ScoreBoostCoroutine(boostMultiplier, duration));
        }

        System.Collections.IEnumerator ScoreBoostCoroutine(float boost, float duration)
        {
            float originalMultiplier = currentMultiplier;
            currentMultiplier = boost;

            Debug.Log($"Score boost active: {boost}x for {duration} seconds");

            yield return new WaitForSeconds(duration);

            currentMultiplier = originalMultiplier;
            Debug.Log("Score boost ended");
        }

        /// <summary>
        /// Get current score
        /// </summary>
        public int GetScore()
        {
            return currentScore;
        }

        /// <summary>
        /// Get high score
        /// </summary>
        public int GetHighScore()
        {
            return highScore;
        }

        /// <summary>
        /// Reset score for new game
        /// </summary>
        public void ResetScore()
        {
            currentScore = 0;
            currentMultiplier = 1f;
            OnScoreChanged?.Invoke(0);
        }

        /// <summary>
        /// Award bonus points (for achievements, challenges, etc.)
        /// </summary>
        public void AwardBonus(int bonusPoints, string reason)
        {
            Debug.Log($"Bonus awarded: +{bonusPoints} ({reason})");
            currentScore += bonusPoints;
            OnScoreChanged?.Invoke(currentScore);
        }

        /// <summary>
        /// Get score tier (for visual effects/rewards)
        /// </summary>
        public int GetScoreTier()
        {
            if (currentScore < 1000) return 0;
            if (currentScore < 5000) return 1;
            if (currentScore < 10000) return 2;
            if (currentScore < 25000) return 3;
            if (currentScore < 50000) return 4;
            if (currentScore < 100000) return 5;
            return 6;
        }

        /// <summary>
        /// Format score for display (with commas)
        /// </summary>
        public string GetFormattedScore()
        {
            return currentScore.ToString("N0");
        }

        /// <summary>
        /// Get score as percentage of high score (for progress bars)
        /// </summary>
        public float GetScoreProgress()
        {
            if (highScore == 0) return 0f;
            return Mathf.Min(1f, (float)currentScore / highScore);
        }
    }
}
