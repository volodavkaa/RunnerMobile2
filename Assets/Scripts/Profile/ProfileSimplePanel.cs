using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileSimplePanel : MonoBehaviour
{
    [Header("UI refs")]
    public TMP_Text infoText;
    public Button   btnPrev, btnNext, btnCreate, btnDelete;

    void OnEnable()           
    {
        UpdateUI();
    }

    

    public void OnPrev()    { Shift(-1); }
    public void OnNext()    { Shift(+1); }
    public void OnCreate()  { ProfileManager.Instance.CreateProfile(); UpdateUI(); }
    public void OnDelete()  { ProfileManager.Instance.DeleteCurrent(); UpdateUI(); }

   

    void Shift(int dir)
    {
        var pm = ProfileManager.Instance;
        int newIndex = (pm.CurrentIndex + dir + pm.Count) % pm.Count;
        pm.SwitchProfile(newIndex);
        UpdateUI();
    }

    void UpdateUI()
    {
        var pm   = ProfileManager.Instance;
        var prof = pm.Current;

        infoText.text =
            $"<b>{prof.profileName}</b>\nCoins: {prof.coins}";

       
        btnDelete.interactable = pm.Count > 1;
        btnCreate .interactable = pm.Count < 5;
    }
}
