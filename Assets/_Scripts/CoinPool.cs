using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CoinPool : MonoBehaviour
{
    [SerializeField] private Coin[] coins;
    [SerializeField] private int maxCoinCount ;
    [SerializeField] private bool isAutoExpand = false;

    public ObjectPool<Coin> coinPool;
    [Inject] private DiContainer _container;

    private void Start()
    {
        coinPool = new ObjectPool<Coin>(coins, maxCoinCount, _container);
        coinPool.isScaleble = isAutoExpand;
    }
}
