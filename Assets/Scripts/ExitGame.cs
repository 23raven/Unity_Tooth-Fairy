using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Exit pressed");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ⭐ остановка в редакторе
#else
        Application.Quit(); // ⭐ выход из билда / web / app
#endif
    }
}