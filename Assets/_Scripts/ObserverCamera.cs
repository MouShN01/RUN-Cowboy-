using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class ObserverCamera : MonoBehaviour
{
    private PlayerController _player;

    [Inject]
    public void Construct(PlayerController player)
    {
        _player = player;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(_player.transform.position.x / 2, transform.position.y, transform.position.z);
        transform.LookAt(_player.transform);
    }
}
