using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource audioSource;
    public AudioClip clickSound;
public AudioClip coinPickupSound;
    void Awake()
{
    var existing = FindObjectsOfType<SoundManager>();
    if (existing.Length > 1)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
}



    public void PlayClickSound()
    {
        Debug.Log($"[SFX] Click! AudioSource = {audioSource}, Clip = {clickSound}");

        if (clickSound != null && audioSource != null)
            audioSource.PlayOneShot(clickSound);
    }
public void PlayCoinSound()
{
    if (coinPickupSound != null && audioSource != null)
        audioSource.PlayOneShot(coinPickupSound);
}

}
