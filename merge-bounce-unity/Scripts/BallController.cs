using UnityEngine;

namespace MergeBounce
{
    /// <summary>
    /// Controls individual ball physics, movement, and collision detection
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BallController : MonoBehaviour
    {
        [Header("Ball Properties")]
        public int value = 2; // 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096
        public BallType ballType = BallType.Normal;
        public PowerUpType powerUpType = PowerUpType.None;

        [Header("Physics Settings")]
        public float launchPower = 15f;
        public float maxVelocity = 20f;
        public float bounciness = 0.7f;
        public float dragInAir = 0.1f;

        [Header("Visual Settings")]
        public Color ballColor = Color.white;
        public GameObject trailEffect;
        public ParticleSystem hitParticles;

        [Header("Audio")]
        public AudioClip bounceSound;
        public AudioClip mergeSound;

        // Components
        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;
        private SpriteRenderer spriteRenderer;
        private TrailRenderer trailRenderer;

        // State
        private bool isLaunched = false;
        private bool hasMerged = false;
        private Vector2 aimDirection;
        private float aimPower;

        // References
        private MergeSystem mergeSystem;
        private GameJuice gameJuice;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            trailRenderer = GetComponent<TrailRenderer>();

            // Configure physics
            rb.gravityScale = 1.5f;
            rb.drag = dragInAir;

            // Set collision properties
            PhysicsMaterial2D bouncyMaterial = new PhysicsMaterial2D();
            bouncyMaterial.bounciness = bounciness;
            bouncyMaterial.friction = 0.3f;
            circleCollider.sharedMaterial = bouncyMaterial;

            // Get references
            mergeSystem = FindObjectOfType<MergeSystem>();
            gameJuice = FindObjectOfType<GameJuice>();

            // Initialize visual
            UpdateVisual();
        }

        void Start()
        {
            // Initially kinematic until launched
            rb.isKinematic = true;
        }

        void Update()
        {
            // Limit max velocity
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }

            // Check if ball fell off screen
            if (transform.position.y < -10f)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Set aim direction (called while dragging)
        /// </summary>
        public void SetAim(Vector2 direction, float power)
        {
            aimDirection = direction.normalized;
            aimPower = Mathf.Clamp(power, 0f, 1f);

            // Show trajectory preview
            // TODO: Draw dotted line showing trajectory
        }

        /// <summary>
        /// Launch the ball
        /// </summary>
        public void Launch()
        {
            if (isLaunched) return;

            isLaunched = true;
            rb.isKinematic = false;

            // Apply force
            Vector2 force = aimDirection * aimPower * launchPower;
            rb.AddForce(force, ForceMode2D.Impulse);

            // Enable trail
            if (trailRenderer != null)
            {
                trailRenderer.enabled = true;
            }

            Debug.Log($"Ball launched with force: {force.magnitude}");
        }

        /// <summary>
        /// Launch with auto-calculated direction (for AI/testing)
        /// </summary>
        public void LaunchAuto(Vector2 direction, float power = 1f)
        {
            SetAim(direction, power);
            Launch();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (hasMerged) return; // Already merged, ignore collisions

            // Check what we hit
            if (collision.gameObject.CompareTag("Peg"))
            {
                HandlePegCollision(collision);
            }
            else if (collision.gameObject.CompareTag("Ball"))
            {
                HandleBallCollision(collision);
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                HandleGroundCollision(collision);
            }
        }

        void HandlePegCollision(Collision2D collision)
        {
            // Play hit effects
            if (gameJuice != null)
            {
                gameJuice.OnPegHit(collision.contacts[0].point);
            }

            // Play sound
            if (bounceSound != null && AudioManager.Instance != null)
            {
                float pitch = Random.Range(0.9f, 1.1f); // Slight pitch variation
                AudioManager.Instance.PlaySound(bounceSound, pitch);
            }

            // Spawn hit particles
            if (hitParticles != null)
            {
                ParticleSystem particles = Instantiate(hitParticles, collision.contacts[0].point, Quaternion.identity);
                particles.Play();
                Destroy(particles.gameObject, 2f);
            }
        }

        void HandleBallCollision(Collision2D collision)
        {
            BallController otherBall = collision.gameObject.GetComponent<BallController>();

            if (otherBall == null || otherBall.hasMerged) return;

            // Check if can merge (same value)
            if (this.value == otherBall.value && !this.hasMerged && !otherBall.hasMerged)
            {
                // Determine which ball triggers the merge (lower instance ID)
                if (gameObject.GetInstanceID() < otherBall.gameObject.GetInstanceID())
                {
                    TriggerMerge(otherBall);
                }
            }
        }

        void HandleGroundCollision(Collision2D collision)
        {
            // Ball settled on ground
            // Check if game over (ball reached top of screen after settling)
            if (transform.position.y > 8f) // Adjust threshold based on your screen size
            {
                GameManager.Instance?.EndGame();
            }
        }

        void TriggerMerge(BallController otherBall)
        {
            if (hasMerged || otherBall.hasMerged) return;

            // Mark both as merged
            hasMerged = true;
            otherBall.hasMerged = true;

            // Calculate merge position (midpoint between balls)
            Vector3 mergePosition = (transform.position + otherBall.transform.position) / 2f;

            // Notify merge system
            if (mergeSystem != null)
            {
                int newValue = value * 2;
                mergeSystem.CreateMerge(value, newValue, mergePosition);
            }

            // Play merge effects
            if (gameJuice != null)
            {
                gameJuice.OnMerge(mergePosition, value);
            }

            // Play merge sound
            if (mergeSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(mergeSound);
            }

            // Destroy both balls
            Destroy(otherBall.gameObject, 0.1f); // Small delay for effects
            Destroy(gameObject, 0.1f);
        }

        void UpdateVisual()
        {
            // Update color based on value
            ballColor = GetColorForValue(value);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = ballColor;
            }

            // Update size based on value (larger numbers = larger balls)
            float scale = GetScaleForValue(value);
            transform.localScale = Vector3.one * scale;
        }

        Color GetColorForValue(int val)
        {
            // Color progression: Red -> Orange -> Yellow -> Green -> Blue -> Purple -> Gold
            switch (val)
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
                case 1024: return Color.white; // White with rainbow effect
                case 2048: return new Color(1f, 0.84f, 0f); // Platinum Gold
                case 4096: return Color.black; // Black hole effect
                default: return Color.white;
            }
        }

        float GetScaleForValue(int val)
        {
            // Balls grow slightly as values increase
            return 0.5f + (Mathf.Log(val, 2) * 0.05f);
        }

        /// <summary>
        /// Execute power-up effect
        /// </summary>
        public void ExecutePowerUp()
        {
            switch (powerUpType)
            {
                case PowerUpType.Bomb:
                    ExecuteBombPowerUp();
                    break;
                case PowerUpType.Rainbow:
                    ExecuteRainbowPowerUp();
                    break;
                case PowerUpType.Mega:
                    ExecuteMegaPowerUp();
                    break;
                case PowerUpType.Ghost:
                    ExecuteGhostPowerUp();
                    break;
                default:
                    Debug.LogWarning("No power-up to execute");
                    break;
            }
        }

        void ExecuteBombPowerUp()
        {
            // Explode and merge all nearby balls
            Debug.Log("BOMB POWER-UP!");

            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, 3f);

            foreach (Collider2D col in nearbyObjects)
            {
                if (col.gameObject.CompareTag("Ball") && col.gameObject != gameObject)
                {
                    BallController nearbyBall = col.GetComponent<BallController>();
                    if (nearbyBall != null)
                    {
                        // Force merge regardless of value
                        TriggerMerge(nearbyBall);
                    }
                }
            }
        }

        void ExecuteRainbowPowerUp()
        {
            // Acts as any value - merges with first ball it touches
            Debug.Log("RAINBOW POWER-UP!");
            // Implementation: Set a flag that allows merging with any value
        }

        void ExecuteMegaPowerUp()
        {
            // Large ball that knocks others around violently
            Debug.Log("MEGA POWER-UP!");
            rb.mass = 5f; // Much heavier
            launchPower *= 2f;
        }

        void ExecuteGhostPowerUp()
        {
            // Passes through first 3 obstacles
            Debug.Log("GHOST POWER-UP!");
            // Implementation: Temporarily disable collision with pegs
        }

        public void SetValue(int newValue)
        {
            value = newValue;
            UpdateVisual();
        }

        public void SetPowerUp(PowerUpType type)
        {
            ballType = BallType.PowerUp;
            powerUpType = type;
            UpdateVisual();
        }
    }

    public enum BallType
    {
        Normal,
        PowerUp,
        Special
    }

    public enum PowerUpType
    {
        None,
        Bomb,
        Rainbow,
        Mega,
        Ghost,
        Clone,
        Wildcard,
        BlackHole,
        Golden
    }
}
