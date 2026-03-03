using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Остановить Play Mode
#else
            Application.Quit(); // Закрыть приложение
#endif
    }
}
