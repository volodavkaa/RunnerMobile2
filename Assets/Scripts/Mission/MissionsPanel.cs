using UnityEngine;
using TMPro;

public class MissionsPanel : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text infoText;                 

    void OnEnable() => Refresh();

    void Refresh()
    {
        var p = ProfileManager.Instance.Current;
        if (p == null) { infoText.text = "No profile"; return; }
        infoText.text = "";
        foreach (var m in p.missions)
{
    var template = MissionManager.Instance.GetTemplate(m.id);
    string display = template.displayName;
    string progress = $"{m.progress}/{template.target}";

    if (m.completed)
        infoText.text += $"<s>{display}: {progress}</s>\n";
    else
        infoText.text += $"{display}: {progress}\n";
}

    }
}
