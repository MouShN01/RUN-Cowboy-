using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ObjectPool<T> where T : MonoBehaviour
{
    [SerializeField] private T prefab;
    [SerializeField] private T[] prefabs;
    [SerializeField] public bool isScaleble;
    [SerializeField] private int poolSize;
    [SerializeField] private Transform container;

    private List<T> pool;
    private DiContainer _diContainer;
    
    public ObjectPool(T[] prefabs, int size, DiContainer diContainer)
    {
        this.prefabs = prefabs;
        this.poolSize = size;
        this.container = null;
        _diContainer = diContainer;

        this.CreatePool();
    }
    
    public ObjectPool(T prefab, int size, DiContainer diContainer)
    {
        this.prefab = prefab;
        this.poolSize = size;
        this.container = null;
        _diContainer = diContainer;

        this.CreatePool();
    }
    
    public ObjectPool(T prefab, int size, Transform container)
    {
        this.prefab = prefab;
        this.poolSize = size;
        this.container = container;
        
        this.CreatePool();
    }

    private void CreatePool()
    {
        pool = new List<T>();

        for (int i = 0; i < poolSize; i++)
        {
            this.CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        int randIndex = Random.Range(0, prefabs.Length);
        var createdObject = _diContainer.InstantiatePrefabForComponent<T>(prefabs[randIndex], container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        this.pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (this.HasFreeElement(out var element))
        {
            return element;
        }

        if (this.isScaleble)
        {
            return this.CreateObject(true);
        }

        throw new Exception("All objects are active now");
    }
}