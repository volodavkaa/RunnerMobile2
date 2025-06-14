using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Статичні перешкоди")]
    public string[] staticTags;                 
    [Header("Рухомі авто")]
    public string[] movingTags;                 
    [Range(0,1)] public float waveChance = .55f;
    public int   minWaveCars = 1;               
    public int   maxWaveCars = 3;
    public float carSpeed    = 12f;             
    //
    public float[] laneX = { -2.5f, 0f, 2.5f };

  
    public void SpawnObstacle(Vector3 platformPos)
{
    TrySpawnMovingWave(platformPos);
    SpawnStatic(platformPos);        
}



    bool TrySpawnMovingWave(Vector3 pos)
{
    if (Random.value > waveChance) return false;

    int cars = Random.Range(minWaveCars, maxWaveCars + 1);
    int centre = Random.Range(0, laneX.Length);
    int left   = Mathf.Max(0, centre - 1);
    int right  = Mathf.Min(laneX.Length - 1, centre + 1);

    List<int> lanes = new() { centre };
    if (cars >= 2) lanes.Add(Random.value < .5f ? left : right);
    if (cars == 3) lanes.Add(left + right - centre);

    HashSet<int> usedLanes = new HashSet<int>();

    foreach (int lane in lanes)
    {
        if (usedLanes.Contains(lane)) continue;

        string tag = movingTags[Random.Range(0, movingTags.Length)];
        float zOffset = Random.Range(4f, 12f);
        Vector3 p = pos + new Vector3(laneX[lane], 0.1f, zOffset);

        Quaternion rot = Quaternion.Euler(0, 180, 0);
        GameObject obj = ObstaclePool.Instance.SpawnFromPool(tag, p, rot);
        if (obj == null) continue;

        usedLanes.Add(lane);
        float speedVar = Random.Range(-2f, 2f);
        MovingObstacle mo = obj.GetComponent<MovingObstacle>();
        if (mo) mo.Init(carSpeed + speedVar);
    }

    return true;
}



    public float minZSpacing = 4f;
private List<Vector3> lastPos = new();

    void SpawnStatic(Vector3 platformPos)
    {
        int amount = Random.value < .3f ? 2 : 1;

        List<int> used = new();
        for (int i = 0; i < amount; i++)
        {
            int lane;
            do lane = Random.Range(0, laneX.Length);
            while (used.Contains(lane) && used.Count < laneX.Length);
            used.Add(lane);

            Vector3 p = platformPos +
                         new Vector3(laneX[lane], .1f, Random.Range(3f, 6f));

            bool close = false;
            foreach (var lp in lastPos)
                if (Vector3.Distance(p, lp) < minZSpacing) { close = true; break; }
            if (close) continue;

            string tag = staticTags[Random.Range(0, staticTags.Length)];
            ObstaclePool.Instance.SpawnFromPool(tag, p, Quaternion.identity);
            lastPos.Add(p);
        }
        if (lastPos.Count > 10)
            lastPos.RemoveRange(0, lastPos.Count - 10);
    }
}
