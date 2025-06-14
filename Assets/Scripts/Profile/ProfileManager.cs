using System.Collections.Generic;
using UnityEngine;
using System;
public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance { get; private set; }
 public List<ProfileData> All => profiles; 
   
    private const int    MAX_PROFILES   = 5;
    private const string PREFS_KEY      = "RunnerProfiles";
    private const string CURRENT_INDEX  = "RunnerCurrentProfile";

    public  List<ProfileData> profiles  = new();
    public  ProfileData       Current   => profiles[currentIndex];
    private int               currentIndex;
    public event Action OnProfileChanged;

   
    public int Count => profiles.Count;   
public int CurrentIndex => currentIndex;
    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else                  { Destroy(gameObject); return; }

        LoadAllProfiles();
        
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) SaveAllProfiles();
    }
    void OnApplicationQuit() => SaveAllProfiles();

    /* ---------- API ---------- */

    public void AddCoins(int amount)    { Current.coins += amount; SaveAllProfiles(); }
    public bool SpendCoins(int amount)
    {
        if (Current.coins < amount) return false;
        Current.coins -= amount; SaveAllProfiles(); return true;
    }

     void NotifyProfileChanged()
    {
        OnProfileChanged?.Invoke();           
        UIManager ui = UIManager.Instance;    
        if (ui != null) ui.RefreshCoins();
    }
    public void CreateProfile(string name = null)
    {
        if (profiles.Count >= MAX_PROFILES) return;

        var p = new ProfileData();
        p.profileName = string.IsNullOrWhiteSpace(name) ? $"Player {profiles.Count + 1}" : name;
        profiles.Add(p);
        SwitchProfile(profiles.Count - 1);
        SaveAllProfiles();
        NotifyProfileChanged();
        MessagePanel.Instance.ShowMessage($"Profile <b>{p.profileName}</b> created!");
    }

    public void DeleteProfile(int index)
    {
        if (index < 0 || index >= profiles.Count) return;
        profiles.RemoveAt(index);
        if (profiles.Count == 0) CreateProfile("Player 1");
        SwitchProfile(Mathf.Clamp(currentIndex, 0, profiles.Count - 1));
        SaveAllProfiles();
        
    }

    public void SwitchProfile(int index)
    {
        if (index < 0 || index >= profiles.Count) return;
        currentIndex = index;
        PlayerPrefs.SetInt(CURRENT_INDEX, currentIndex);
        PlayerPrefs.Save();
        NotifyProfileChanged();
         MessagePanel.Instance.ShowMessage($"Switched to <b>{Current.profileName}</b>");
    }


    public void LoadAllProfiles()
    {
        profiles.Clear();

        string json = PlayerPrefs.GetString(PREFS_KEY, string.Empty);
        if (!string.IsNullOrEmpty(json))
            profiles.AddRange(JsonUtility.FromJson<Wrapper>(json).items);

        if (profiles.Count == 0)                      
            profiles.Add(new ProfileData { profileName = "Player 1" });

        currentIndex = PlayerPrefs.GetInt(CURRENT_INDEX, 0);
        currentIndex = Mathf.Clamp(currentIndex, 0, profiles.Count - 1);
    }
    public void DeleteCurrent()                      
    {
        DeleteProfile(currentIndex);
        NotifyProfileChanged();
        
    }

    public void SetCurrent(int index)                
    {
        SwitchProfile(index);
         SaveAllProfiles();
    OnProfileChanged?.Invoke();
    }
    public void SaveAllProfiles()
    {
        var wrap = new Wrapper { items = profiles.ToArray() };
        string json = JsonUtility.ToJson(wrap);
        PlayerPrefs.SetString(PREFS_KEY, json);
        PlayerPrefs.SetInt(CURRENT_INDEX, currentIndex);
        PlayerPrefs.Save();
    }


    [System.Serializable] private class Wrapper { public ProfileData[] items; }
}
