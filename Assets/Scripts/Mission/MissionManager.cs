
using System;
using UnityEngine;
using System.Collections.Generic; 
public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    [SerializeField] MissionCatalog catalog;
    [SerializeField] int missionsPerDay = 4;

    ProfileData P => ProfileManager.Instance.Current;
Dictionary<MissionKind,int> runCounters = new();
    void Awake()
    {
        if (Instance) { Destroy(gameObject); return; }
        Instance = this;
    }


    void Start()
{
    EnsureTodayMissions();
    UIManager.Instance?.RefreshCoins();

    ProfileManager.Instance.OnProfileChanged += EnsureTodayMissions;
    if (GameManager.Instance != null)
        GameManager.Instance.StartedGame += StartRun;
}

    void EnsureTodayMissions()
    {
        int today = DateTime.Now.Year * 10000 +
                    DateTime.Now.Month * 100 +
                    DateTime.Now.Day;
        if (P.missions.Exists(m => m.dayStamp == today)) return;

        // прибираємо старі місії
        P.missions.Clear();

        // випадкові N шаблонів
        var pool = new System.Random();
        while (P.missions.Count < missionsPerDay)
        {
            var t = catalog.all[pool.Next(catalog.all.Length)];
            if (P.missions.Exists(m => m.id == t.id)) continue; 
            P.missions.Add(new MissionInstance
            {
                id = t.id,
                progress = 0,
                completed = false,
                dayStamp = today
            });
        }
        ProfileManager.Instance.SaveAllProfiles();
    }
    public void RegisterJump()
        => AddProgress(MissionKind.JumpTimes, 1);

    public void RegisterDistance(int meters, bool sameRun)
    {
        AddProgress(MissionKind.RunDistance, meters);
        if (sameRun)
            AddProgress(MissionKind.RunDistanceSingleRun, meters);
    }
public void RegisterCoin(int amount, bool sameRun)
{
    P.totalCoinsCollected += amount;
    AddProgress(MissionKind.CollectCoinsTotal, amount);
    if (sameRun) AddProgress(MissionKind.CollectCoinsRun, amount);
}

public void RegisterLoseHeart()
{
    P.totalHeartsLost += 1;
    AddProgress(MissionKind.LoseHearts, 1);
}

public void RegisterDeath()
{
    P.totalDeaths += 1;
    AddProgress(MissionKind.DieRuns, 1);
}
void StartRun()
{
    runCounters.Clear();
}
    void AddProgress(MissionKind kind, int value)
{
    foreach (var m in P.missions)
    {
        var t = GetTemplate(m.id);
        if (t.kind != kind || m.completed) continue;
       if (t.singleRun)
       {
          if (!runCounters.ContainsKey(kind)) runCounters[kind] = 0;
           runCounters[kind] += value;
           m.progress = runCounters[kind];
       }
       else
       {
           m.progress += value;
       }

        if (m.progress >= t.target)
        {
            m.completed = true;
            P.coins += t.reward;
            MessagePanel.Instance.ShowMessage(
                $"Mission completed! +{t.reward} coins");
        }
    }
}

    public MissionTemplate GetTemplate(string id)
     => Array.Find(catalog.all, t => t.id == id);
     void OnProfileChanged()
{
    EnsureTodayMissions();     
}
}
