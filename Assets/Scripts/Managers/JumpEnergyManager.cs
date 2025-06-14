using UnityEngine;
using UnityEngine.UI;

public class JumpEnergyManager : MonoBehaviour
{
    public static JumpEnergyManager Instance;

    public int maxEnergy = 3;
    public int restoreCost = 50;
    private int currentEnergy;

    [Header("UI")]
    public Image[] energyIcons; 

    public Button restoreButton; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateUI();
        restoreButton.gameObject.SetActive(false);
        restoreButton.onClick.AddListener(RestoreEnergy);
    }

    public bool UseEnergy()
    {
        if (currentEnergy <= 0) { MessagePanel.Instance.ShowMessage("No energy! Tap + to restore."); return false; }

        currentEnergy--;
        UpdateUI();

        if (currentEnergy == 0)
            restoreButton.gameObject.SetActive(true);

        return true;
    }

    public void RestoreEnergy()
    {
        bool purchased = UIManager.Instance.SpendCoins(restoreCost);

        if (!purchased)
        {
            MessagePanel.Instance.ShowMessage("Not enough coins!");
            return;
        }

        currentEnergy = maxEnergy;
        restoreButton.gameObject.SetActive(false);
        UpdateUI();

        MessagePanel.Instance.ShowMessage($"Energy restored! (-{restoreCost})");
    }

    void UpdateUI()
{
    for (int i = 0; i < energyIcons.Length; i++)
    {
        energyIcons[i].gameObject.SetActive(i < currentEnergy);
    }

    restoreButton.gameObject.SetActive(currentEnergy == 0);
}


    public bool HasEnergy()
    {
        return currentEnergy > 0;
    }
}
