using TMPro;
using UnityEngine;
using Zenject;

public class StatPresenter : MonoBehaviour
{
    private PlayerStats _playerStats;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameoverUI;
    [SerializeField] private RetryWindow retryUI;

    [Inject]
    public void Construct(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }

    private void Start()
    {
        _playerStats.CoinsUpdated += OnCoinsChanged;
        _playerStats.ScoreUpdated += OnScoreChanged;
        _playerStats.Loose += OnLooseChanged;
        _playerStats.Hit += OnHitChanged;
    }

    private void OnCoinsChanged()
    {
        coinText.text = _playerStats.Coins.ToString();
    }

    private void OnScoreChanged()
    {
        scoreText.text = _playerStats.Score.ToString();
    }

    private void OnHitChanged()
    {
        retryUI.Show(
            onRetry: () => _playerStats.ContinueAfterHit(),
            onGiveUp: () => OnLooseChanged()
        );
    }

    private void OnLooseChanged()
    {
        gameoverUI.SetActive(true);
    }

    public void OnRetryAccepted()
    {
        Time.timeScale = 1f;
        retryUI.gameObject.SetActive(false);
        _playerStats.ContinueAfterHit(); // метод для восстановления игры
    }

    public void OnRetryDeclined()
    {
        retryUI.gameObject.SetActive(false);
        OnLooseChanged(); // показать GameOver
    }
}
