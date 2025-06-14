using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Texts (optional)")]
    public TMP_Text coinsText;
    public TMP_Text metersText;

    [Header("Distance Icons")]
    public Image  distanceIcon;
    public Sprite iconStage1;
    public Sprite iconStage2;
    public Sprite iconStage3;

    private int meters = 0;

    void Awake()
{
    if (Instance == null) Instance = this;
    else { Destroy(gameObject); return; }
}
void Start()            
{
    UpdateCoinsUI();
}
    public void UpdateMeters(int newMeters)
    {
        meters = newMeters;

        if (metersText != null)
            metersText.text = meters + " м";

        if (distanceIcon == null) return;

        if      (meters < 500)  distanceIcon.sprite = iconStage1;
        else if (meters < 1000) distanceIcon.sprite = iconStage2;
        else                    distanceIcon.sprite = iconStage3;
    }

    public void AddCoin(int amount)
    {
        ProfileManager.Instance.AddCoins(amount);
        UpdateCoinsUI();
    }
    public bool SpendCoins(int amount)
    {
        bool ok = ProfileManager.Instance.SpendCoins(amount);
        if (ok) UpdateCoinsUI();
        return ok;
    }

    void UpdateCoinsUI()
    {
        if (coinsText == null) return;

        var pm = ProfileManager.Instance;
        if (pm == null || pm.Current == null) return;

        coinsText.text = pm.Current.coins.ToString();
    }
public void RefreshCoins()
{
    if (coinsText != null)
        coinsText.text = ProfileManager.Instance.Current.coins.ToString();
}


}
