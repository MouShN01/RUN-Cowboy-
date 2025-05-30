using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button shopBtn;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text money;
    [SerializeField] private Button moeyAdBtn;

    private AdManager _adManager;

    [Inject]
    public void Construct(AdManager adManager)
    {
        _adManager = adManager;
    }

    private void Start()
    {
        playBtn.onClick.AddListener(LoadGame);
        moeyAdBtn.onClick.AddListener(ShowAd);
        money.text = $"Coins: {Stats.Instance.Progress.TotalCoins}";
        score.text = $"Best: {Stats.Instance.Progress.MaxScore}";
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    private void ShowAd()
    {
        _adManager.ShowRewardedAd();
    }

}
