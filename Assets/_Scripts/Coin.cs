using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using Zenject;

public class Coin : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    private PickSoundManager _pickSound;
    private PlayerStats _playerStats;
    public Tile tile;

    [Inject]
    public void Construct(PlayerStats playerStats, PickSoundManager pickSoundManager)
    {
        _playerStats = playerStats;
        _pickSound = pickSoundManager;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        particleSystem.Play();
        if (_pickSound == null)
        {
            Debug.LogWarning("PickSoundManager is not assigned.");
        }
        else
        { 
            _pickSound.PlayCoinSound();
        }
                
        StartCoroutine(DisableAfterDelay(0.1f));
        gameObject.transform.SetParent(null);
        tile.coins.Remove(this);
        
        _playerStats.AddCoin();
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}

