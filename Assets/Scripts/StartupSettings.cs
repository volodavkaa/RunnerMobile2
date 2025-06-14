using UnityEngine;

public class StartupSettings : MonoBehaviour
{
    [Tooltip("FPS LOCK")]
    public int targetFrameRate = 60;

    void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0; 
    }
}
