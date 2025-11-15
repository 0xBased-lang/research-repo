using UnityEngine;
using System.Collections.Generic;

namespace MergeBounce
{
    /// <summary>
    /// Handles merge detection, creation, and merge chain tracking
    /// </summary>
    public class MergeSystem : MonoBehaviour
    {
        [Header("Ball Prefabs")]
        public GameObject ballPrefab;
        public Dictionary<int, GameObject> ballPrefabsByValue;

        [Header("Merge Settings")]
        public float mergeDelay = 0.1f;
        public Vector3 spawnPosition = new Vector3(0, 5, 0);

        [Header("Visual Effects")]
        public GameObject mergeEffectPrefab;
        public GameObject numberPopupPrefab;

        // Events
        public System.Action<int, int> OnMerge; // fromValue, toValue

        // Internal tracking
        private List<MergeData> pendingMerges = new List<MergeData>();
        private int currentMergeChain = 0;
        private float lastMergeTime = 0f;
        private const float CHAIN_TIME_WINDOW = 3f; // 3 seconds to maintain chain

        void Update()
        {
            // Check if merge chain has expired
            if (currentMergeChain > 0 && Time.time - lastMergeTime > CHAIN_TIME_WINDOW)
            {
                Debug.Log($"Merge chain broken. Total chain: {currentMergeChain}");
                currentMergeChain = 0;
            }
        }

        /// <summary>
        /// Create a merge between two balls
        /// </summary>
        public void CreateMerge(int fromValue, int toValue, Vector3 position)
        {
            // Track chain
            currentMergeChain++;
            lastMergeTime = Time.time;

            Debug.Log($"Merge #{currentMergeChain}: {fromValue} -> {toValue} at {position}");

            // Create merge effect
            if (mergeEffectPrefab != null)
            {
                GameObject effect = Instantiate(mergeEffectPrefab, position, Quaternion.identity);
                Destroy(effect, 2f);
            }

            // Show number popup
            if (numberPopupPrefab != null)
            {
                GameObject popup = Instantiate(numberPopupPrefab, position, Quaternion.identity);
                // TODO: Set popup text to "+{toValue}"
                Destroy(popup, 1.5f);
            }

            // Delay spawn to allow effects to play
            Invoke(nameof(SpawnMergedBall), mergeDelay);
            pendingMerges.Add(new MergeData { value = toValue, position = position });

            // Notify listeners
            OnMerge?.Invoke(fromValue, toValue);
        }

        void SpawnMergedBall()
        {
            if (pendingMerges.Count == 0) return;

            MergeData merge = pendingMerges[0];
            pendingMerges.RemoveAt(0);

            // Create new ball with merged value
            GameObject newBall = CreateBall(merge.value, merge.position);

            // Apply slight upward force to make it "pop" into existence
            Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            }

            Debug.Log($"Spawned merged ball: {merge.value}");
        }

        /// <summary>
        /// Create a ball with specific value at position
        /// </summary>
        public GameObject CreateBall(int value, Vector3 position)
        {
            GameObject ball;

            // Check if we have a specific prefab for this value
            if (ballPrefabsByValue != null && ballPrefabsByValue.ContainsKey(value))
            {
                ball = Instantiate(ballPrefabsByValue[value], position, Quaternion.identity);
            }
            else
            {
                // Use generic ball prefab
                ball = Instantiate(ballPrefab, position, Quaternion.identity);

                // Set value
                BallController controller = ball.GetComponent<BallController>();
                if (controller != null)
                {
                    controller.SetValue(value);
                }
            }

            return ball;
        }

        /// <summary>
        /// Spawn a ball at the spawn position (for player control)
        /// </summary>
        public GameObject SpawnBallAtTop(int value)
        {
            return CreateBall(value, spawnPosition);
        }

        /// <summary>
        /// Check if two balls can merge (same value)
        /// </summary>
        public bool CanMerge(int value1, int value2)
        {
            return value1 == value2;
        }

        /// <summary>
        /// Get the next value after merge
        /// </summary>
        public int GetMergedValue(int currentValue)
        {
            return currentValue * 2;
        }

        /// <summary>
        /// Check if value has reached maximum (4096)
        /// </summary>
        public bool IsMaxValue(int value)
        {
            return value >= 4096;
        }

        /// <summary>
        /// Get current merge chain count
        /// </summary>
        public int GetCurrentChain()
        {
            return currentMergeChain;
        }

        /// <summary>
        /// Reset merge chain (called when chain timer expires or game ends)
        /// </summary>
        public void ResetChain()
        {
            currentMergeChain = 0;
            lastMergeTime = 0f;
        }

        /// <summary>
        /// Get time remaining in current chain
        /// </summary>
        public float GetChainTimeRemaining()
        {
            if (currentMergeChain == 0) return 0f;
            return Mathf.Max(0f, CHAIN_TIME_WINDOW - (Time.time - lastMergeTime));
        }

        /// <summary>
        /// Check if ball value is valid
        /// </summary>
        public bool IsValidBallValue(int value)
        {
            // Valid values: 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096
            return value == 2 || value == 4 || value == 8 || value == 16 ||
                   value == 32 || value == 64 || value == 128 || value == 256 ||
                   value == 512 || value == 1024 || value == 2048 || value == 4096;
        }

        /// <summary>
        /// Get color for ball value (for visual coding)
        /// </summary>
        public Color GetColorForValue(int value)
        {
            // Same color scheme as BallController
            switch (value)
            {
                case 2: return new Color(1f, 0.3f, 0.3f); // Red
                case 4: return new Color(1f, 0.6f, 0.2f); // Orange
                case 8: return new Color(1f, 0.9f, 0.2f); // Yellow
                case 16: return new Color(0.3f, 1f, 0.3f); // Green
                case 32: return new Color(0.2f, 0.6f, 1f); // Blue
                case 64: return new Color(0.7f, 0.3f, 1f); // Purple
                case 128: return new Color(1f, 0.5f, 0.8f); // Pink
                case 256: return new Color(1f, 0.84f, 0f); // Gold
                case 512: return new Color(0.75f, 0.75f, 0.75f); // Silver
                case 1024: return Color.white; // White
                case 2048: return new Color(1f, 0.84f, 0f); // Platinum
                case 4096: return Color.black; // Black hole
                default: return Color.white;
            }
        }
    }

    [System.Serializable]
    public class MergeData
    {
        public int value;
        public Vector3 position;
    }
}
