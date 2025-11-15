using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MergeBounce
{
    /// <summary>
    /// Manages all UI elements and screens
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Screens")]
        public GameObject mainMenuScreen;
        public GameObject gamePlayScreen;
        public GameObject pauseMenuScreen;
        public GameObject gameOverScreen;
        public GameObject leaderboardScreen;
        public GameObject shopScreen;

        [Header("HUD Elements")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI comboText;
        public Image comboTimerBar;
        public TextMeshProUGUI ballCountText;
        public TextMeshProUGUI coinsText;
        public TextMeshProUGUI gemsText;

        [Header("Game Over")]
        public TextMeshProUGUI finalScoreText;
        public TextMeshProUGUI rankText;
        public TextMeshProUGUI statsText;

        [Header("Notifications")]
        public GameObject notificationPrefab;
        public Transform notificationParent;

        [Header("Extreme Fever")]
        public GameObject extremeFeverPanel;
        public TextMeshProUGUI extremeFeverText;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            ShowMainMenu();
        }

        // Screen Management
        public void ShowMainMenu()
        {
            HideAllScreens();
            mainMenuScreen?.SetActive(true);
        }

        public void ShowGameUI()
        {
            HideAllScreens();
            gamePlayScreen?.SetActive(true);
        }

        public void ShowPauseMenu()
        {
            pauseMenuScreen?.SetActive(true);
        }

        public void HidePauseMenu()
        {
            pauseMenuScreen?.SetActive(false);
        }

        public void ShowGameOver(GameStats stats)
        {
            HideAllScreens();
            gameOverScreen?.SetActive(true);

            if (finalScoreText != null)
                finalScoreText.text = $"Score: {stats.finalScore:N0}";

            if (statsText != null)
            {
                statsText.text = $"Merges: {stats.mergeCount}\n" +
                                $"Highest Combo: {stats.highestCombo}x\n" +
                                $"Extreme Fever: {stats.extremeFeverCount}x\n" +
                                $"Time: {stats.gameTime:F1}s";
            }
        }

        public void ShowLeaderboard()
        {
            HideAllScreens();
            leaderboardScreen?.SetActive(true);
        }

        public void ShowShop()
        {
            HideAllScreens();
            shopScreen?.SetActive(true);
        }

        void HideAllScreens()
        {
            mainMenuScreen?.SetActive(false);
            gamePlayScreen?.SetActive(false);
            pauseMenuScreen?.SetActive(false);
            gameOverScreen?.SetActive(false);
            leaderboardScreen?.SetActive(false);
            shopScreen?.SetActive(false);
        }

        // HUD Updates
        public void UpdateScore(int score)
        {
            if (scoreText != null)
                scoreText.text = $"{score:N0}";
        }

        public void UpdateCombo(int comboLevel)
        {
            if (comboText != null)
            {
                if (comboLevel > 0)
                {
                    comboText.text = $"{comboLevel}x COMBO!";
                    comboText.gameObject.SetActive(true);
                }
                else
                {
                    comboText.gameObject.SetActive(false);
                }
            }
        }

        public void UpdateComboTimer(float progress)
        {
            if (comboTimerBar != null)
                comboTimerBar.fillAmount = progress;
        }

        public void UpdateBallCount(int count)
        {
            if (ballCountText != null)
                ballCountText.text = $"Balls: {count}";
        }

        public void UpdateCoins(int coins)
        {
            if (coinsText != null)
                coinsText.text = $"{coins}";
        }

        public void UpdateGems(int gems)
        {
            if (gemsText != null)
                gemsText.text = $"{gems}";
        }

        // Special Effects
        public void ShowExtremeFever()
        {
            if (extremeFeverPanel != null)
            {
                extremeFeverPanel.SetActive(true);
                if (extremeFeverText != null)
                    extremeFeverText.text = "🎉 EXTREME FEVER! 🎉";
            }
        }

        public void HideExtremeFever()
        {
            if (extremeFeverPanel != null)
                extremeFeverPanel.SetActive(false);
        }

        public void ShowRank(int rank)
        {
            if (rankText != null)
                rankText.text = $"Global Rank: #{rank}";
        }

        public void ShowPowerUpNotification(PowerUpType type)
        {
            ShowNotification($"Power-Up: {type}!", 2f);
        }

        public void ShowNotification(string message, float duration = 3f)
        {
            if (notificationPrefab != null && notificationParent != null)
            {
                GameObject notification = Instantiate(notificationPrefab, notificationParent);
                TextMeshProUGUI text = notification.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                    text.text = message;

                Destroy(notification, duration);
            }
        }

        // Button Handlers
        public void OnPlayButtonClicked()
        {
            GameManager.Instance?.StartGame();
        }

        public void OnPauseButtonClicked()
        {
            GameManager.Instance?.PauseGame();
        }

        public void OnResumeButtonClicked()
        {
            GameManager.Instance?.ResumeGame();
        }

        public void OnRestartButtonClicked()
        {
            GameManager.Instance?.StartGame();
        }

        public void OnMainMenuButtonClicked()
        {
            GameManager.Instance?.ReturnToMenu();
        }
    }
}
