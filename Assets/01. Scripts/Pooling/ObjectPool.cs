using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [System.Serializable]
    private class Pool
    {
        public string prefabName;
        public GameObject prefab;
        public int initialSize = 10;
        public Queue<GameObject> poolQueue = new Queue<GameObject>();
    }

    [SerializeField]
    private List<Pool> pools = new List<Pool>();

    private Dictionary<string, Pool> nameToPoolMap = new Dictionary<string, Pool>();

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var pool in pools)
        {
            if (!nameToPoolMap.ContainsKey(pool.prefabName))
            {
                nameToPoolMap.Add(pool.prefabName, pool);

                for (int i = 0; i < pool.initialSize; i++)
                {
                    GameObject obj = CreateNewObject(pool);
                    obj.SetActive(false);
                    pool.poolQueue.Enqueue(obj);
                }
            }           
        }
    }

    private GameObject CreateNewObject(Pool pool)
    {
        GameObject obj = Instantiate(pool.prefab);
        obj.name = pool.prefabName;
        obj.transform.SetParent(this.transform);
        return obj;
    }

    public GameObject GetObject(string prefabName)
    {
        if (!nameToPoolMap.ContainsKey(prefabName))
        {
            return null;
        }

        Pool pool = nameToPoolMap[prefabName];

        if (pool.poolQueue.Count > 0)
        {
            GameObject obj = pool.poolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return CreateNewObject(pool);
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);

        foreach (var pool in pools)
        {
            if (pool.prefabName == obj.name.Replace("(Clone)", "").Trim())
            {
                pool.poolQueue.Enqueue(obj);
                return;
            }
        }
    }
}