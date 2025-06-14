using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Heart Purchase")]
    public TMP_Text heartPriceText;

    const int maxHearts = 10;

    void Start()
{
    UpdateHeartPriceUI();
    ProfileManager.Instance.OnProfileChanged += UpdateHeartPriceUI;
}
    public void BuyHeart()
    {
        var profile = ProfileManager.Instance.Current;
        int bought  = profile.heartsBought;

        if (bought >= maxHearts)
        {
            MessagePanel.Instance.ShowMessage("Maximum hearts reached.");
            return;
        }

        int price  = (int)Mathf.Pow(2, bought) * 400;

        if (!ProfileManager.Instance.SpendCoins(price))
        {
            MessagePanel.Instance.ShowMessage("Not enough coins!");
            return;
        }
        profile.heartsBought = bought + 1;
        profile.bonusHearts  += 1;
        ProfileManager.Instance.SaveAllProfiles();
        UIManager.Instance.RefreshCoins();  

        MessagePanel.Instance.ShowMessage("Heart purchased ❤️");
        UpdateHeartPriceUI();
    }

    void UpdateHeartPriceUI()
    {
        var profile = ProfileManager.Instance.Current;
        int bought  = profile.heartsBought;

        if (bought >= maxHearts)
            heartPriceText.text = "Buy Heart: MAX";
        else
        {
            int price = (int)Mathf.Pow(2, bought) * 400;
heartPriceText.text = $"Buy Heart ({price} coins)";

        }
    }
}
