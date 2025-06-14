using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public int numberOfPlatforms = 5;
    public float platformLength = 10f;
    public Transform player;

    public ObstacleSpawner obstacleSpawner;
    public GameObject coinPrefab;

    private List<GameObject> platforms = new List<GameObject>();
    private float spawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (player.position.z - 15f > spawnZ - numberOfPlatforms * platformLength)
        {
            MovePlatform();
        }
    }
private int spawnedCount = 0;
    void SpawnPlatform()
{
    GameObject obj = Instantiate(platformPrefab, Vector3.forward * spawnZ, Quaternion.identity);
    platforms.Add(obj);

    SpawnCoins(obj.transform.position);

    if (spawnedCount >= 2) 
        obstacleSpawner.SpawnObstacle(obj.transform.position);

    spawnZ += platformLength;
    spawnedCount++;
}

    void MovePlatform()
{
    GameObject platform = platforms[0];
    platforms.RemoveAt(0);
    platform.transform.position = Vector3.forward * spawnZ;
    platforms.Add(platform);

    SpawnCoins(platform.transform.position);

    if (spawnedCount >= 2)
        obstacleSpawner.SpawnObstacle(platform.transform.position);

    spawnZ += platformLength;
    spawnedCount++;
}

    void SpawnCoins(Vector3 platformPos)
{
    float[] lanes = new float[] { -2.5f, 0f, 2.5f }; 
    float startZ = platformPos.z + 1f;
    float endZ = platformPos.z + platformLength - 1f;

    int coinsPerLane = 3;

    foreach (float x in lanes)
    {
        for (int i = 0; i < coinsPerLane; i++)
        {
            float z = Mathf.Lerp(startZ, endZ, i / (float)(coinsPerLane - 1));
            Vector3 coinPos = new Vector3(x, 1f, z);

        
            if (!Physics.CheckSphere(coinPos, 0.3f, LayerMask.GetMask("Obstacle")))
            {
                CoinPool.Instance.Spawn(coinPos);

            }
        }
    }
}

}
