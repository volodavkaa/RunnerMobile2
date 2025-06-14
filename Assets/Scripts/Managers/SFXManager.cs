using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    public AudioSource audioSource;
    public AudioClip buttonClick;

    void Awake()
    {
        Instance = this;
    }

    public void PlayClick()
{
    Debug.Log("Click Sound Played");
    if (buttonClick != null && audioSource != null)
        audioSource.PlayOneShot(buttonClick);
}

}
