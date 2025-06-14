using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;      
    public GameObject shopPanel;      
    public GameObject profilesPanel; 
    public GameObject missionsPanel; 

    public void PlayGame()
    {
        PlayerPrefs.SetInt("StartFromMenu", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }

    public void OpenShop()     => ShowOnly(shopPanel);
    public void OpenProfiles() => ShowOnly(profilesPanel);
    public void ExitGame()     => Application.Quit();

    public void CloseShop()     => ShowOnly(mainPanel);
    public void CloseProfiles() => ShowOnly(mainPanel);


    void Start()   => ShowOnly(mainPanel);  
public void OpenMissions()
    {
        missionsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void CloseMissions()
    {
        missionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    void ShowOnly(GameObject panelToShow)
    {
        mainPanel    .SetActive(false);
        shopPanel    .SetActive(false);
        profilesPanel.SetActive(false);
        panelToShow?.SetActive(true);
    }
}
