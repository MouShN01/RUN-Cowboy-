using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource coinSource;

    public void PlayCoinSound()
    {
        if (coinSource == null)
        {
            Debug.LogWarning("Coin audio source is not assigned.");
            return;
        }
        coinSource.Play();
    }
}
