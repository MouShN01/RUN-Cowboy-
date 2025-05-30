using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    private BannerView bannerView;
    private RewardedAd rewardedAd;

    private Action _onRewardGranted;
    private Action _onAdClosed;
    private bool _grantCoins;

    private int rewardAmount = 10;

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob initialized");
            LoadBannerAd();
            LoadRewardedAd();
        });
    }

    // === Banner ===
    void LoadBannerAd()
    {
        string bannerId = "ca-app-pub-3940256099942544/6300978111"; // Тестовый
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
        bannerView.LoadAd(new AdRequest());
    }

    // === Rewarded ===
    void LoadRewardedAd()
    {
        string rewardedId = "ca-app-pub-3940256099942544/5224354917"; // Тестовый

        RewardedAd.Load(rewardedId, new AdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Ошибка загрузки наградной рекламы: " + error);
                return;
            }

            rewardedAd = ad;

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Реклама закрыта. Загружаем снова.");
                _onAdClosed?.Invoke(); // Вот тут вызывается респаун
                LoadRewardedAd();
            };

            rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Реклама не показалась: " + error);
            };
        });
    }

    public void ShowRewardedAd(Action onRewardGranted = null, Action onAdClosed = null, bool grantCoins = true)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            _onRewardGranted = onRewardGranted;
            _onAdClosed = onAdClosed;
            _grantCoins = grantCoins;

            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Пользователь досмотрел рекламу.");
                if (_grantCoins)
                {
                    Stats.Instance.AddCoins(rewardAmount);
                    Debug.Log($"Выдана награда: {reward.Type}, {reward.Amount}");
                }

                _onRewardGranted?.Invoke();
            });
        }
        else
        {
            Debug.LogWarning("Наградная реклама не загружена");
        }
    }
}
