using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private Obstacle[] obstacles;
    [SerializeField] private int maxTileCount ;
    [SerializeField] private bool isAutoExpand = false;

    public ObjectPool<Obstacle> obstaclePool;
    [Inject] private DiContainer _container;

    private void Start()
    {
        obstaclePool = new ObjectPool<Obstacle>(obstacles, maxTileCount, _container);
        obstaclePool.isScaleble = isAutoExpand;
    }
}
