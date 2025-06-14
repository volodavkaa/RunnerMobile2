using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public static CoinPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public Pool coinPool;

    private Queue<GameObject> poolQueue;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        poolQueue = new Queue<GameObject>();

        for (int i = 0; i < coinPool.size; i++)
        {
            GameObject obj = Instantiate(coinPool.prefab);
            obj.SetActive(false);
            obj.name = coinPool.tag;
            poolQueue.Enqueue(obj);
        }
    }

    public GameObject Spawn(Vector3 position)
    {
        if (poolQueue.Count == 0) return null;

        GameObject obj = poolQueue.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
        poolQueue.Enqueue(obj); 
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
    }
}
