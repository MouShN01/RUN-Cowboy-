using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public enum TileType
    {
        plane,
        rightPit,
        leftPit,
        fullPit
    }
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileSpawner tileSpawner;
        [SerializeField] private float speed;
        [SerializeField] private TileType tileType;
        private PlayerStats _playerStats;
        public bool firstSlot;
        public bool secondSlot;
        public bool thirdSlot;
        
        public Dictionary<float, bool> _availableXValues = new Dictionary<float, bool>()
        {
            {-0.75f, true},
            {0, true},
            {0.75f, true}
        };

        public TileType GetTileType => tileType;

        public List<Obstacle> obstacles;
        public List<Coin> coins;

        public event Action disabled;

        [Inject]
        private void Construct(TileSpawner tileSpawner, PlayerStats playerStats)
        {
            this.tileSpawner = tileSpawner;
            _playerStats = playerStats;
        }

        private void Awake()
        {
            switch (tileType)
            {
                case TileType.plane:
                    SetTileSlotsAccess(true, true, true);
                    break;
                case TileType.fullPit:
                    SetTileSlotsAccess(false, false, false);
                    break;
                case TileType.leftPit:
                    SetTileSlotsAccess(false, false, true);
                    break;
                case TileType.rightPit:
                    SetTileSlotsAccess(true, false, false);
                    break;
            }
        }

        private void Start()
        {
            _playerStats.Loose += OnLooseChanged;
        }

        private void FixedUpdate()
        {
            transform.Translate(Vector3.back * (speed * Time.deltaTime));
            firstSlot = _availableXValues[-0.75f];
            secondSlot = _availableXValues[0];
            thirdSlot = _availableXValues[0.75f];
        }

        private void OnTriggerExit(Collider other)
        {
            StartCoroutine(DeactivateAfterDelay(1.0f));
            tileSpawner.SpawnTile();
        }

        public float ZWidth()
        {
            float minZ = float.MaxValue;
            float maxZ = float.MinValue;

            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                minZ = Mathf.Min(minZ, renderer.bounds.min.z);
                maxZ = Mathf.Max(maxZ, renderer.bounds.max.z);
            }

            return maxZ - minZ;
        }

        public float XTilePositionToSpawnObstacle()
        {
            List<float> availableKeys = _availableXValues
                .Where(kv => kv.Value == true)
                .Select(kv => kv.Key)
                .ToList();
            switch (tileType)
            {
                case TileType.plane:
                    int randomIndex = Random.Range(0, availableKeys.Count());
                    float randomKey = availableKeys[randomIndex];
                    _availableXValues[randomKey] = false;
                    return randomKey;
                case TileType.leftPit:
                    return availableKeys.FirstOrDefault(k=> k == 0.75f);
                case TileType.rightPit:
                    return availableKeys.FirstOrDefault(k=> k == -0.75f);
                default:
                    return 0;
            }
        }
        
        public float XTilePositionToSpawnCoins()
        {
            List<float> availableKeys = _availableXValues.Keys.Select(key => key).ToList();
            
            int randomIndex = Random.Range(0, availableKeys.Count());
            return availableKeys[randomIndex];
        }

        public bool IsSlotAvailable(float slotKey)
        {
            //Debug.Log($"{_availableXValues[-0.75f]}, {_availableXValues[0]}, {_availableXValues[0.75f]}");
            return _availableXValues[slotKey];
        }

        public void SetTileSlotsAccess(bool firstSlot, bool secondSlot, bool thirdSlot)
        {
            List<float> keysList = _availableXValues.Keys.ToList();
            _availableXValues[keysList[0]] = firstSlot;
            _availableXValues[keysList[1]] = secondSlot;
            _availableXValues[keysList[2]] = thirdSlot;
        }

        public void ClearTile()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.gameObject.SetActive(false);
                obstacle.transform.SetParent(null);
            }
            obstacles.Clear();
            foreach (var coin in coins)
            {
                coin.gameObject.SetActive(false);
                coin.transform.SetParent(null);
            }
            coins.Clear();
            if(tileType != TileType.plane) return;
            List<float> keys = _availableXValues.Keys.ToList();
            foreach (var key in keys)
            {
                _availableXValues[key] = true;
            }
        }

        private IEnumerator DeactivateAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
            ClearTile();
            _playerStats.AddPoint(1);
        }

        private void OnLooseChanged()
        {
            Debug.Log("Tile.OnLooseChanged called");
            speed = 0;
        }
    }
}
