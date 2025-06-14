using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public PauseMenu pauseMenuController;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
{
    if (isPaused) return;

    Time.timeScale = 0f;
    isPaused = true;
    if (pauseMenuController != null)
        StartCoroutine(pauseMenuController.Show());
}

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseMenuController != null)
            StartCoroutine(pauseMenuController.Hide());
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
