using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip coinPickupSound;
public AudioClip jumpSound;
public AudioClip laneSwitchSound;
public AudioClip slideSound;
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
        if (clickSound != null && audioSource != null)
            audioSource.PlayOneShot(clickSound);
    }
    public void PlayCoinSound()
    {
        if (coinPickupSound != null && audioSource != null)
            audioSource.PlayOneShot(coinPickupSound);
    }
public void PlayJumpSound() {
    if (jumpSound != null) audioSource.PlayOneShot(jumpSound);
}

public void PlayLaneSwitchSound() {
    if (laneSwitchSound != null) audioSource.PlayOneShot(laneSwitchSound);
}

public void PlaySlideSound() {
    if (slideSound != null) audioSource.PlayOneShot(slideSound);
}

}
