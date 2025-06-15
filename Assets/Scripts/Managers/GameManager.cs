using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
public  event Action StartedGame; 
    [Header("UI")]
    public GameObject gameOverPanel;
    public TMP_Text   livesText;
    public GameObject startPanel;
    public HeartsUI   heartsUI;
[Header("Gameplay HUD")]
    public GameObject hudCanvas; 
    int   lives        = 3;
    bool  isGameOver   = false;
    bool  isGameStarted = false;
public TMP_Text distanceText;
public TMP_Text rewardText;
public TMP_Text recordText;
bool newRecordAnnounced = false;

Vector3 startPos;
float distanceTraveled = 0f;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (PlayerPrefs.GetInt("StartFromMenu", 0) == 1)
        {
            PlayerPrefs.SetInt("StartFromMenu", 0);
            StartGame();
        }
        else
        {
            Time.timeScale = 0f;                
        }
    }

    public void StartGame()
{
    if (isGameStarted) return;
startPos = GameObject.FindGameObjectWithTag("Player").transform.position;

    isGameStarted = true;
    Time.timeScale = 1f;
hudCanvas?.SetActive(true);  
    int bonusHearts = ProfileManager.Instance.Current.bonusHearts;
    lives = 3 + bonusHearts;

    UpdateLivesUI();
    heartsUI?.InitHearts(lives);
    StartedGame?.Invoke();  
}

    public void LoseLife()
    {
        if (isGameOver || !isGameStarted) return;

        lives--;
        heartsUI?.LoseHeart();
MissionManager.Instance?.RegisterLoseHeart();
        UpdateLivesUI();

        if (lives <= 0) GameOver();
    }

    public void GameOver()
{
    if (isGameOver) return;

    isGameOver = true;
    MissionManager.Instance?.RegisterDeath();
    Time.timeScale = 0f;
    hudCanvas?.SetActive(false);
    gameOverPanel?.SetActive(true);

    int coinReward = Mathf.FloorToInt(distanceTraveled / 5);
    ProfileManager.Instance.AddCoins(coinReward);

    float previousBest = ProfileManager.Instance.Current.bestDistance;
    if (distanceTraveled > previousBest)
    {
        ProfileManager.Instance.Current.bestDistance = distanceTraveled;
        MessagePanel.Instance.ShowMessage($"🏅 New Record: {Mathf.FloorToInt(distanceTraveled)} m!");
        ProfileManager.Instance.AddCoins(20); 
    }

    
    ProfileManager.Instance.SaveAllProfiles();
    if (distanceText) distanceText.text = $"Distance: {Mathf.FloorToInt(distanceTraveled)} m";
    if (rewardText)   rewardText.text   = $"+{coinReward} coins";
    if (recordText)   recordText.text   = $"Record: {Mathf.FloorToInt(ProfileManager.Instance.Current.bestDistance)} m";
}
void Update()
{
    if (isGameStarted && !isGameOver)
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        distanceTraveled = playerPos.z - startPos.z;
        if (!newRecordAnnounced &&
            distanceTraveled > ProfileManager.Instance.Current.bestDistance)
        {
            newRecordAnnounced = true;
            MessagePanel.Instance.ShowMessage("🏅 New distance record!");
            ProfileManager.Instance.AddCoins(20);
        }
    }
}


    public void Restart()
    {
        PlayerPrefs.SetInt("StartFromMenu", 1);
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        hudCanvas?.SetActive(true); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        ProfileManager.Instance.SaveAllProfiles();
        Time.timeScale = 1f;
        hudCanvas?.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
    void UpdateLivesUI()
    {
        if (livesText) livesText.text = $"Lives: {lives}";
    }
    public bool IsGameStarted() => isGameStarted;
}
