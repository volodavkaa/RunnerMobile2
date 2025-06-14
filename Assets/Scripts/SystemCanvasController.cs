using UnityEngine;

public class SystemCanvasController : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);   
    }
}
