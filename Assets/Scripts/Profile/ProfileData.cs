using System;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class MissionInstance          
{
    public string id;                 
    public int    progress;
    public bool   completed;
    public int    dayStamp;           
}
[Serializable]                          
public class ProfileData
{
    public string profileName = "Player";
    public int coins = 0;
     public int heartsBought = 0;
     public int bonusHearts = 0;

    public int totalCoinsCollected = 0;
    public int totalHeartsLost    = 0;
    public int totalDeaths        = 0;
    public float bestDistance = 0f;

 
     public List<MissionInstance> missions = new List<MissionInstance>();
}
