using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenuUI;


    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f; // Остановить время
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f; // Вернуть время
        }
    }
}