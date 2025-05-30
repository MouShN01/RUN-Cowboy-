using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using Zenject;

public class TilePool : MonoBehaviour
{
    [SerializeField] private Tile[] tiles;
    [SerializeField] private int maxTileCount ;
    [SerializeField] private bool isAutoExpand = false;

    public ObjectPool<Tile> tilePool;
    [Inject] private DiContainer _container;

    private void Start()
    {
        tilePool = new ObjectPool<Tile>(tiles, maxTileCount, _container);
        tilePool.isScaleble = isAutoExpand;
    }
}
