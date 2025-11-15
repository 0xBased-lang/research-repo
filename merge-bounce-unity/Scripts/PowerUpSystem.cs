using UnityEngine;

namespace MergeBounce
{
    /// <summary>
    /// Manages power-up spawning, selection, and execution
    /// </summary>
    public class PowerUpSystem : MonoBehaviour
    {
        [Header("Power-Up Prefabs")]
        public GameObject bombBallPrefab;
        public GameObject rainbowBallPrefab;
        public GameObject megaBallPrefab;
        public GameObject ghostBallPrefab;
        public GameObject cloneBallPrefab;
        public GameObject wildcardBallPrefab;
        public GameObject blackHoleBallPrefab;
        public GameObject goldenBallPrefab;

        [Header("Spawn Settings")]
        public Vector3 spawnPosition = new Vector3(0, 5, 0);
        public float goldenBallChance = 0.01f; // 1% chance

        // Power-up weights for random selection
        private PowerUpWeight[] powerUpWeights = new PowerUpWeight[]
        {
            new PowerUpWeight { type = PowerUpType.Bomb, weight = 20 },
            new PowerUpWeight { type = PowerUpType.Rainbow, weight = 20 },
            new PowerUpWeight { type = PowerUpType.Mega, weight = 15 },
            new PowerUpWeight { type = PowerUpType.Ghost, weight = 15 },
            new PowerUpWeight { type = PowerUpType.Clone, weight = 10 },
            new PowerUpWeight { type = PowerUpType.Wildcard, weight = 10 },
            new PowerUpWeight { type = PowerUpType.BlackHole, weight = 5 },
            new PowerUpWeight { type = PowerUpType.Golden, weight = 1 } // Rare
        };

        /// <summary>
        /// Spawn a random power-up ball
        /// </summary>
        public GameObject SpawnPowerUpBall()
        {
            // Check for golden ball (rare)
            if (Random.value < goldenBallChance)
            {
                return SpawnSpecificPowerUp(PowerUpType.Golden);
            }

            // Select random power-up based on weights
            PowerUpType selectedType = SelectRandomPowerUp();

            return SpawnSpecificPowerUp(selectedType);
        }

        /// <summary>
        /// Spawn a specific power-up type
        /// </summary>
        public GameObject SpawnSpecificPowerUp(PowerUpType type)
        {
            GameObject powerUpBall = null;

            switch (type)
            {
                case PowerUpType.Bomb:
                    powerUpBall = Instantiate(bombBallPrefab, spawnPosition, Quaternion.identity);
                    break;
                case PowerUpType.Rainbow:
                    powerUpBall = Instantiate(rainbowBallPrefab, spawnPosition, Quaternion.identity);
                    break;
                case PowerUpType.Mega:
                    powerUpBall = Instantiate(megaBallPrefab, spawnPosition, Quaternion.identity);
                    break;
                case PowerUpType.Ghost:
                    powerUpBall = Instantiate(ghostBallPrefab, spawnPosition, Quaternion.identity);
                    break;
                case PowerUpType.Clone:
                    powerUpBall = Instantiate(cloneBallPrefab, spawnPosition, Quaternion.identity);
                    break;
                case PowerUpType.Wildcard:
                    powerUpBall = Instantiate(wildcardBallPrefab, spawnPosition, Quaternion.identity);
                    break;
                case PowerUpType.BlackHole:
                    powerUpBall = Instantiate(blackHoleBallPrefab, spawnPosition, Quaternion.identity);
                    break;
                case PowerUpType.Golden:
                    powerUpBall = Instantiate(goldenBallPrefab, spawnPosition, Quaternion.identity);
                    break;
            }

            if (powerUpBall != null)
            {
                BallController controller = powerUpBall.GetComponent<BallController>();
                if (controller != null)
                {
                    controller.SetPowerUp(type);
                }

                Debug.Log($"Spawned power-up: {type}");

                // Show notification
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowPowerUpNotification(type);
                }
            }

            return powerUpBall;
        }

        /// <summary>
        /// Select random power-up based on weights
        /// </summary>
        PowerUpType SelectRandomPowerUp()
        {
            int totalWeight = 0;
            foreach (var pw in powerUpWeights)
            {
                totalWeight += pw.weight;
            }

            int randomValue = Random.Range(0, totalWeight);
            int currentWeight = 0;

            foreach (var pw in powerUpWeights)
            {
                currentWeight += pw.weight;
                if (randomValue < currentWeight)
                {
                    return pw.type;
                }
            }

            return PowerUpType.Bomb; // Fallback
        }

        /// <summary>
        /// Get power-up description for UI
        /// </summary>
        public string GetPowerUpDescription(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.Bomb:
                    return "💣 Explodes and merges all nearby balls!";
                case PowerUpType.Rainbow:
                    return "🌈 Merges with any ball it touches!";
                case PowerUpType.Mega:
                    return "⚡ Giant ball that smashes through everything!";
                case PowerUpType.Ghost:
                    return "👻 Passes through obstacles!";
                case PowerUpType.Clone:
                    return "🎭 Splits into two balls!";
                case PowerUpType.Wildcard:
                    return "🎲 Random effect - surprise!";
                case PowerUpType.BlackHole:
                    return "🌑 Pulls nearby balls toward it!";
                case PowerUpType.Golden:
                    return "👑 5x SCORE + Auto-merge everything!";
                default:
                    return "Special ball!";
            }
        }

        /// <summary>
        /// Get power-up icon name for UI
        /// </summary>
        public string GetPowerUpIconName(PowerUpType type)
        {
            return $"PowerUp_{type}";
        }

        /// <summary>
        /// Execute bomb power-up effect
        /// </summary>
        public void ExecuteBombEffect(Vector3 position, float radius = 3f)
        {
            Debug.Log($"BOMB explosion at {position}!");

            // Find all balls in radius
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, radius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Ball"))
                {
                    BallController ball = hitCollider.GetComponent<BallController>();
                    if (ball != null)
                    {
                        // Force merge or destroy
                        Destroy(hitCollider.gameObject, 0.1f);
                    }
                }
            }

            // Spawn explosion effect
            // TODO: Add particle effect
        }

        /// <summary>
        /// Execute black hole effect (attract nearby balls)
        /// </summary>
        public void ExecuteBlackHoleEffect(Vector3 position, float pullStrength = 5f)
        {
            Collider2D[] nearbyBalls = Physics2D.OverlapCircleAll(position, 5f);

            foreach (var col in nearbyBalls)
            {
                if (col.CompareTag("Ball"))
                {
                    Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 direction = ((Vector2)position - rb.position).normalized;
                        rb.AddForce(direction * pullStrength, ForceMode2D.Force);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class PowerUpWeight
    {
        public PowerUpType type;
        public int weight;
    }
}
