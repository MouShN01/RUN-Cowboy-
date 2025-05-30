using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameoverWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button homeBtn;
    [SerializeField] private GameOverAnimator animator;
    private PlayerStats _playerStats;

    [Inject]
    public void Construct(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }

    private void Start()
    {
        scoreLabel.text = $"Your score: {_playerStats.Score}";
        restartBtn.onClick.AddListener(RestartScene);
        homeBtn.onClick.AddListener(LoadMainMenu);
    }

    private void OnEnable()
    {
        animator.AnimateUI();
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
