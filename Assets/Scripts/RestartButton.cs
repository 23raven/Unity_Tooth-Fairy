using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartLevel()
    {
        Debug.Log("Restart pressed");

        Time.timeScale = 1f; // ⭐ обязательно вернуть время!

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}