using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    public static ObstaclePool Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, GameObject> prefabByTag;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        prefabByTag = new Dictionary<string, GameObject>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.name = pool.tag; 
                objectPool.Enqueue(obj);
            }

            poolDictionary[pool.tag] = objectPool;
            prefabByTag[pool.tag] = pool.prefab;
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"ObstaclePool: Тег '{tag}' не знайдено.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        poolDictionary[tag].Enqueue(obj); 

        return obj;
    }

    public GameObject GetPrefab(string tag)
    {
        return prefabByTag.ContainsKey(tag) ? prefabByTag[tag] : null;
    }
    public void ReturnObstacle(GameObject obj)
    {
        obj.SetActive(false);
    }
}
