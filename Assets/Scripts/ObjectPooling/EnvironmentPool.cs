using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPool : MonoBehaviour
{
    public static EnvironmentPool Instance;

    [System.Serializable]
    public class PoolItem
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<PoolItem> items;
    private Dictionary<string, Queue<GameObject>> pools;
    private Dictionary<string, GameObject> prefabLookup; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        pools = new Dictionary<string, Queue<GameObject>>();
        prefabLookup = new Dictionary<string, GameObject>();

        foreach (var item in items)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < item.size; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                obj.name = item.tag; 
                objectPool.Enqueue(obj);
            }

            pools.Add(item.tag, objectPool);
            prefabLookup[item.tag] = item.prefab;
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(tag))
        {
            Debug.LogWarning("Pool doesn't exist: " + tag);
            return null;
        }

        GameObject obj = pools[tag].Dequeue();

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        pools[tag].Enqueue(obj);

        return obj;
    }
    public Quaternion GetPrefabRotation(string tag)
    {
        if (!prefabLookup.ContainsKey(tag)) return Quaternion.identity;
        return prefabLookup[tag].transform.rotation;
    }
}
