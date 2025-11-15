using UnityEngine;

namespace MergeBounce
{
    /// <summary>
    /// Manages combo tracking, multipliers, and Extreme Fever mode
    /// </summary>
    public class ComboManager : MonoBehaviour
    {
        [Header("Combo Settings")]
        public float comboTimeWindow = 3f; // Seconds to maintain combo
        public int extremeFeverThreshold = 5; // 5x combo triggers Extreme Fever
        public float extremeFeverDuration = 10f;

        [Header("Multiplier Settings")]
        public float[] multipliers = { 1f, 2f, 3f, 4f, 5f, 7f, 10f }; // Per combo level

        // State
        private int currentComboLevel = 0;
        private float lastMergeTime = 0f;
        private bool isExtremeFeverActive = false;
        private float extremeFeverEndTime = 0f;

        // Events
        public System.Action<int> OnComboChanged; // comboLevel
        public System.Action OnExtremeFever;
        public System.Action OnExtremeFeverEnd;

        void Update()
        {
            // Check combo timeout
            if (currentComboLevel > 0 && Time.time - lastMergeTime > comboTimeWindow)
            {
                BreakCombo();
            }

            // Check Extreme Fever timeout
            if (isExtremeFeverActive && Time.time > extremeFeverEndTime)
            {
                EndExtremeFever();
            }
        }

        /// <summary>
        /// Called when a merge happens
        /// </summary>
        public void OnMerge()
        {
            currentComboLevel++;
            lastMergeTime = Time.time;

            Debug.Log($"Combo Level: {currentComboLevel}x");

            // Check for Extreme Fever trigger
            if (currentComboLevel >= extremeFeverThreshold && !isExtremeFeverActive)
            {
                TriggerExtremeFever();
            }

            OnComboChanged?.Invoke(currentComboLevel);
        }

        /// <summary>
        /// Break the current combo
        /// </summary>
        public void BreakCombo()
        {
            if (currentComboLevel > 0)
            {
                Debug.Log($"Combo broken at {currentComboLevel}x");
                currentComboLevel = 0;
                OnComboChanged?.Invoke(0);
            }
        }

        /// <summary>
        /// Reset combo to zero
        /// </summary>
        public void ResetCombo()
        {
            currentComboLevel = 0;
            lastMergeTime = 0f;
            OnComboChanged?.Invoke(0);
        }

        /// <summary>
        /// Trigger Extreme Fever mode (Peggle-style celebration)
        /// </summary>
        void TriggerExtremeFever()
        {
            isExtremeFeverActive = true;
            extremeFeverEndTime = Time.time + extremeFeverDuration;

            Debug.Log("🎉 EXTREME FEVER MODE! 🎉");

            // Trigger celebration effects
            OnExtremeFever?.Invoke();

            // Slow down time slightly for dramatic effect
            Time.timeScale = 0.8f;
        }

        /// <summary>
        /// End Extreme Fever mode
        /// </summary>
        void EndExtremeFever()
        {
            isExtremeFeverActive = false;

            Debug.Log("Extreme Fever ended");

            // Restore normal time
            Time.timeScale = 1f;

            OnExtremeFeverEnd?.Invoke();
        }

        /// <summary>
        /// Get current combo level
        /// </summary>
        public int GetComboLevel()
        {
            return currentComboLevel;
        }

        /// <summary>
        /// Get current score multiplier based on combo
        /// </summary>
        public float GetMultiplier()
        {
            if (isExtremeFeverActive)
            {
                return 10f; // Maximum multiplier during Extreme Fever
            }

            int index = Mathf.Min(currentComboLevel, multipliers.Length - 1);
            return multipliers[index];
        }

        /// <summary>
        /// Get time remaining in current combo
        /// </summary>
        public float GetComboTimeRemaining()
        {
            if (currentComboLevel == 0) return 0f;
            return Mathf.Max(0f, comboTimeWindow - (Time.time - lastMergeTime));
        }

        /// <summary>
        /// Get combo progress (0-1) for UI display
        /// </summary>
        public float GetComboProgress()
        {
            if (currentComboLevel == 0) return 0f;
            return GetComboTimeRemaining() / comboTimeWindow;
        }

        /// <summary>
        /// Check if Extreme Fever is active
        /// </summary>
        public bool IsExtremeFeverActive()
        {
            return isExtremeFeverActive;
        }

        /// <summary>
        /// Get Extreme Fever time remaining
        /// </summary>
        public float GetExtremeFeverTimeRemaining()
        {
            if (!isExtremeFeverActive) return 0f;
            return Mathf.Max(0f, extremeFeverEndTime - Time.time);
        }

        /// <summary>
        /// Force trigger Extreme Fever (for testing/power-ups)
        /// </summary>
        public void ForceExtremeFever()
        {
            TriggerExtremeFever();
        }

        /// <summary>
        /// Extend Extreme Fever duration (for power-ups)
        /// </summary>
        public void ExtendExtremeFever(float additionalSeconds)
        {
            if (isExtremeFeverActive)
            {
                extremeFeverEndTime += additionalSeconds;
            }
        }

        /// <summary>
        /// Get combo level name for display
        /// </summary>
        public string GetComboLevelName()
        {
            switch (currentComboLevel)
            {
                case 0: return "";
                case 1: return "COMBO!";
                case 2: return "2x COMBO!";
                case 3: return "3x CHAIN!";
                case 4: return "4x CHAIN!";
                case 5: return "5x MEGA CHAIN!";
                case 6: return "6x SUPER CHAIN!";
                case 7: return "7x ULTRA CHAIN!";
                case 8: return "8x LEGENDARY!";
                case 9: return "9x GODLIKE!";
                case 10: return "10x UNSTOPPABLE!";
                default: return $"{currentComboLevel}x INSANE!";
            }
        }

        /// <summary>
        /// Get combo color for UI (escalates with level)
        /// </summary>
        public Color GetComboColor()
        {
            if (isExtremeFeverActive)
            {
                // Rainbow/gold during Extreme Fever
                return Color.HSVToRGB((Time.time * 0.5f) % 1f, 0.8f, 1f);
            }

            switch (currentComboLevel)
            {
                case 0: case 1: return Color.white;
                case 2: return Color.yellow;
                case 3: return new Color(1f, 0.6f, 0f); // Orange
                case 4: return new Color(1f, 0.3f, 0f); // Red-orange
                case 5: return Color.red;
                case 6: return new Color(1f, 0f, 0.5f); // Pink-red
                case 7: return new Color(0.8f, 0f, 1f); // Purple
                case 8: return new Color(0f, 0.5f, 1f); // Cyan
                case 9: return new Color(0f, 1f, 0.5f); // Green
                default: return Color.magenta;
            }
        }
    }
}
