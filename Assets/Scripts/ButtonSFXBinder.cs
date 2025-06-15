using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonSFXBinder : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedButtonBinding());
    }

    private IEnumerator DelayedButtonBinding()
    {
        yield return null;

        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (var btn in buttons)
        {
            btn.onClick.RemoveListener(PlayClick);
            btn.onClick.AddListener(PlayClick);
        }
    }

    private void PlayClick()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayClickSound();
    }
}
