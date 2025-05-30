using System;
using System.Collections;
using _Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RetryWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text time;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button giveUpButton;

    private Action _onRetry;
    private Action _onGiveUp;
    private Coroutine _countdownCoroutine;

    private float _countdownSeconds = 3f;

    private AdManager _adManager;
    private PlayerController _playerController;
    private PlayerStats _playerStats;

    [Inject]
    public void Construct(AdManager adManager, PlayerController playerController, PlayerStats playerStats)
    {
        _adManager = adManager;
        _playerController = playerController;
        _playerStats = playerStats;
    }

    public void Show(Action onRetry, Action onGiveUp)
    {
        Debug.Log("RetryWindow.Show() called");
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        _onRetry = onRetry;
        _onGiveUp = onGiveUp;

        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(OnRetryClicked);

        giveUpButton.onClick.RemoveAllListeners();
        giveUpButton.onClick.AddListener(OnGiveUpClicked);

        // Надежно остановим, если уже была корутина
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        _countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        float timeLeft = _countdownSeconds;
        while (timeLeft > 0)
        {
            time.text = Mathf.CeilToInt(timeLeft).ToString();
            yield return new WaitForSecondsRealtime(1f);
            timeLeft -= 1f;
        }

        OnGiveUpClicked();
    }

    private void OnRetryClicked()
    {
        // Останавливаем таймер
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        Time.timeScale = 1f;

        _adManager.ShowRewardedAd(
            onRewardGranted: null,
            onAdClosed: () =>
            {
                gameObject.SetActive(false);
                _playerController.Respawn(false);
                _onRetry?.Invoke();
            },
            grantCoins: false
        );
    }


    private void OnGiveUpClicked()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        Time.timeScale = 1f;
        gameObject.SetActive(false);
        _playerStats.PlayerLoose(); // вызываем метод, который обрабатывает проигрыш игрока
        _onGiveUp?.Invoke();
    }
}
