using System;
using System.Collections.Generic;
using UnityEngine;

public enum MissionKind
{
    JumpTimes,
     RunDistance,
   RunDistanceSingleRun,
    CollectCoinsRun,       
    CollectCoinsTotal,    
    LoseHearts,            
    DieRuns   
}

[Serializable]
public class MissionTemplate
{
    [Tooltip("Назва для UI")]
    public string displayName = "Зроби 10 стрибків";
    [Tooltip("Унікальний ідентифікатор (JUMP_10, DIST_1000_1RUN, …)")]
    public string id = "JUMP_10";

    public MissionKind kind = MissionKind.JumpTimes;

    [Tooltip("Скільки разів / метрів потрібно до виконання")]
    public int target = 10;

    [Tooltip("Скільки монет отримає гравець")]
    public int reward = 25;

    [Tooltip("Чи має виконуватися за один забіг")]
    public bool singleRun = false;

    public override string ToString() => $"{id} (target {target})";
}

[CreateAssetMenu(
    fileName = "MissionCatalog",
    menuName = "Runner/Mission Catalog",
    order = 101)]
public class MissionCatalog : ScriptableObject
{
    [Tooltip("Повний перелік шаблонів, з яких щодня обиратимуться місії")]
    public MissionTemplate[] all;

    public MissionTemplate GetById(string id)
    {
        foreach (var t in all)
            if (t.id == id) return t;
        return null;
    }

    public List<MissionTemplate> PickRandom(int count, System.Random rng = null)
    {
        rng ??= new System.Random();
        var pool   = new List<MissionTemplate>(all);
        var result = new List<MissionTemplate>(count);

        while (result.Count < count && pool.Count > 0)
        {
            int idx = rng.Next(pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }
        return result;
    }
}
