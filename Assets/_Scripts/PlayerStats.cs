using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   private int _coins;
   private int _score;
   private bool _isAlive = true;

   private bool _isHited = false;

   public int Coins => _coins;

   public int Score => _score;

   public bool IsAlive => _isAlive;

   public bool IsHited => _isHited;

   public event Action CoinsUpdated;
   public event Action ScoreUpdated;
   public event Action Loose;
   public event Action Hit;

   private void Start()
   {
      LoadLocal();
   }


   public void AddCoin()
   {
      _coins++;
      UpdateCoinCount();
   }

   public void AddPoint(int pointVal)
   {
      _score += pointVal;
      UpdateScore();
   }

   public void PlayerLoose()
   {
      _isAlive = false;
      SetLoose();
   }

   public void UpdateCoinCount()
   {
      CoinsUpdated?.Invoke();
   }

   public void UpdateScore()
   {
      ScoreUpdated?.Invoke();
   }

   public void SetLoose()
   {
      Debug.Log("PlayerStats: Player has lost the game.");
      Loose?.Invoke();
      Stats.Instance.UpdateProgress(Coins, Score);
   }

   public void SetHit()
   {
      if (_isHited) return;

      _isHited = true;
      Hit?.Invoke();
   }

   private void SaveLocal()
   {
      PlayerPrefs.SetInt("coins", _coins);
      PlayerPrefs.SetInt("score", _score);
      PlayerPrefs.Save();
   }

   private void LoadLocal()
   {
      _coins = PlayerPrefs.GetInt("coins", 0);
      _score = PlayerPrefs.GetInt("score", 0);
   }

   public void ContinueAfterHit()
   {
      // например, восстанавливаем немного жизней или просто продолжаем
      _isHited = false;
      _isAlive = true;
      
      // Вызываем событие или логика сброса положения и т.д.
   }
}

