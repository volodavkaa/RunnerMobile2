using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Canvas Group")]
    public CanvasGroup pauseGroup;

    [Header("Fade Settings")]
    public float fadeDuration = 0.2f;

    void Awake()
    {
        if (pauseGroup == null)
            pauseGroup = GetComponent<CanvasGroup>();

        pauseGroup.alpha = 0f;
        pauseGroup.interactable = false;
        pauseGroup.blocksRaycasts = false;
        pauseGroup.gameObject.SetActive(false);
    }

    public IEnumerator Show()
    {
        pauseGroup.gameObject.SetActive(true);

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            pauseGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }

        pauseGroup.alpha = 1f;
        pauseGroup.interactable = true;
        pauseGroup.blocksRaycasts = true;
    }

    public IEnumerator Hide()
    {
        pauseGroup.interactable = false;
        pauseGroup.blocksRaycasts = false;

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            pauseGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            yield return null;
        }

        pauseGroup.alpha = 0f;
        pauseGroup.gameObject.SetActive(false);
    }
}
