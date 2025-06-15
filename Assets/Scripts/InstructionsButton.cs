using UnityEngine;

public class InstructionsButton : MonoBehaviour
{
    public void OpenInstructions()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string filePath = Application.streamingAssetsPath + "/instructions.html";
        Application.OpenURL(filePath);
#else
        string filePath = "file://" + Application.streamingAssetsPath + "/instructions.html";
        Application.OpenURL(filePath);
#endif
    }
}
