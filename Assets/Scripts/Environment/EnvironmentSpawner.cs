using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Transform player;
    public float spawnAheadDistance = 60f;
    public float despawnBehindDistance = 20f;

    [System.Serializable]
    public class Lane
    {
        public string name;
        public float sideOffset = 5f;
        public float chunkLength = 10f;
        public float yOffset = 0f;
        public string[] poolTags;

        [HideInInspector] public float lastSpawnZ;
        [HideInInspector] public List<GameObject> activeObjects = new List<GameObject>();
    }

    public Lane[] lanes;

    void Start()
    {
        foreach (var lane in lanes)
        {
            lane.lastSpawnZ = player.position.z;
        }
    }

    void Update()
    {
        foreach (var lane in lanes)
        {
            SpawnAhead(lane);
            DespawnBehind(lane);
        }
    }

    void SpawnAhead(Lane lane)
    {
        while (lane.lastSpawnZ < player.position.z + spawnAheadDistance)
        {
            string tag = lane.poolTags[Random.Range(0, lane.poolTags.Length)];
            Vector3 pos = new Vector3(lane.sideOffset, lane.yOffset, lane.lastSpawnZ);

            Quaternion prefabRotation = EnvironmentPool.Instance.GetPrefabRotation(tag);
GameObject obj = EnvironmentPool.Instance.SpawnFromPool(tag, pos, prefabRotation);

            if (obj != null)
            {
                lane.activeObjects.Add(obj);
            }

            lane.lastSpawnZ += lane.chunkLength;
        }
    }

    void DespawnBehind(Lane lane)
    {
        float despawnZ = player.position.z - despawnBehindDistance;

        for (int i = lane.activeObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = lane.activeObjects[i];
            if (obj.transform.position.z < despawnZ)
            {
                obj.SetActive(false);
                lane.activeObjects.RemoveAt(i);
            }
        }
    }
}
