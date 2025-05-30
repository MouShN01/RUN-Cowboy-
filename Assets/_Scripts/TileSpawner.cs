using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
    private TilePool _tilePool;
    private ObstaclePool _obstaclePool;
    private CoinPool _coinPool;
    private Vector3 _nextSpawnPoint;
    private int _numberOfInitialTiles = 10;
    private Tile _lastTile;
    
    private void Start()
    {
        _tilePool = GetComponent<TilePool>();
        _obstaclePool = GetComponent<ObstaclePool>();
        _coinPool = GetComponent<CoinPool>();
        SpawnInitialTile();
        for (int i = 0; i < _numberOfInitialTiles; i++)
        {
            SpawnTile();
        }
    }
    
    public void SpawnTile()
    { 
        Tile tile = _tilePool.tilePool.GetFreeElement();
        tile.transform.position = _lastTile.transform.GetChild(1).transform.position;
        SpawnObstacle(tile);
        SpawnCoins(tile);
        _lastTile = tile;
    }

    private void SpawnInitialTile()
    {
        _lastTile = _tilePool.tilePool.GetFreeElement();
    }

    public void SpawnObstacle(Tile tile)
    {
        if(tile.GetTileType != TileType.plane) return;
        int randomCountOfObstacles = Random.Range(0, 4);
        for (int i = 0; i < randomCountOfObstacles; i++)
        {
            Obstacle obstacle = _obstaclePool.obstaclePool.GetFreeElement();
            obstacle.transform.position = new Vector3(tile.XTilePositionToSpawnObstacle(), obstacle.transform.position.y, tile.transform.position.z + tile.ZWidth()/2);
            obstacle.transform.SetParent(tile.transform);
            obstacle.tile = tile;
            tile.obstacles.Add(obstacle);
        }
    }

    public void SpawnCoins(Tile tile)
    {
        float initXSpawnPos = tile.XTilePositionToSpawnCoins();
        if (tile.IsSlotAvailable(initXSpawnPos))
        {
            for (int i = 0; i < 5; i++)
            {
                Coin coin = _coinPool.coinPool.GetFreeElement();
                coin.transform.position = new Vector3(initXSpawnPos, tile.transform.position.y + 0.2f,tile.transform.position.z+i*0.5f);
                coin.transform.SetParent(tile.transform);
                coin.tile = tile;
                tile.coins.Add(coin);
            }
        }
        else
        {
            float arcHeight = 0.8f;
            float arcWidth = tile.ZWidth()-0.3f;

            for (int i = 0; i < 7; i++)
            {
                float t = (float)i / (7 - 1);

                float zPos = Mathf.Lerp(-arcWidth / 2, arcWidth / 2, t);
                float yPos = arcHeight * Mathf.Sin(Mathf.PI * t);
                Coin coin = _coinPool.coinPool.GetFreeElement();
                coin.transform.position = new Vector3(initXSpawnPos, tile.transform.position.y + yPos+0.2f, tile.transform.position.z + zPos + tile.ZWidth()/2);
                coin.transform.SetParent(tile.transform);
                coin.tile = tile;
                tile.coins.Add(coin);
            }
        }
    }
    
}
