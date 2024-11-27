using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int initialPoolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = CreateNewObject();
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.SetParent(this.transform);
        return obj;
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return CreateNewObject();
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}