using UnityEngine;

public class Stats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static Stats Instance { get; private set; }

    public PlayerProgress Progress { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadProgress();
    }

    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(Progress);
        PlayerPrefs.SetString("PlayerProgress", json);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("PlayerProgress"))
        {
            string json = PlayerPrefs.GetString("PlayerProgress");
            Progress = JsonUtility.FromJson<PlayerProgress>(json);
        }
        else
        {
            Progress = new PlayerProgress();
        }
    }

    public void UpdateProgress(int sessionCoins, int sessionScore)
    {
        Progress.TotalCoins += sessionCoins;
        if (sessionScore > Progress.MaxScore)
            Progress.MaxScore = sessionScore;

        SaveProgress();
    }

    public void AddCoins(int amount)
    {
        Progress.TotalCoins += amount;
        SaveProgress();
    }
}
