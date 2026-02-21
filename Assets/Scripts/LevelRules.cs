using UnityEngine;
using TMPro;

public class LevelRules : MonoBehaviour
{
    public float time = 120f;
    private float startTime;

    public int toothGoal = 20;

    public PlayerInventory player;
    public ToothSpawner[] toothSpawners;

    public TMP_Text resultText;
    public TMP_Text backToBaseText;   // ⭐ новый текст
    public TMP_Text statisticText;    // ⭐ статистика

    public GameObject endGameUI;

    private bool gameEnded = false;
    private bool returnToBase = false;

    public TMP_Text timerText;      // ⭐ таймер сверху
    public TMP_Text toothGoalText;  // ⭐ прогресс зубов

    void Start()
    {
        startTime = time;

        // ⭐ endGameUI выключен в начале
        if (endGameUI != null)
            endGameUI.SetActive(false);

        // BackToBase скрыт на старте
        if (backToBaseText != null)
            backToBaseText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        time -= Time.deltaTime;

        UpdateTimerUI();      // ⭐ новое
        UpdateToothGoalUI();  // ⭐ новое

        // --- ПРОИГРЫШ ---
        if (time <= 0)
        {
            EndGame(false);
            return;
        }

        // --- ЦЕЛЬ ВЫПОЛНЕНА ---
        if (!returnToBase && player.tooth >= toothGoal && time > 0)
        {
            returnToBase = true;

            if (backToBaseText != null)
                backToBaseText.gameObject.SetActive(true);
        }
    }

    // ⭐ вызывается из PlayerInventory когда игрок вошёл в Money
    public void PlayerEnteredBase()
    {
        if (gameEnded) return;

        if (returnToBase)
        {
            EndGame(true);
        }
    }

    void EndGame(bool win)
    {
        gameEnded = true;

        // выключаем BackToBase текст
        if (backToBaseText != null)
            backToBaseText.gameObject.SetActive(false);

        // ⭐ выключаем спавнеры
        foreach (ToothSpawner spawner in toothSpawners)
        {
            if (spawner != null)
                spawner.enabled = false;
        }

        // ⭐ уничтожаем все зубы
        GameObject[] teeth = GameObject.FindGameObjectsWithTag("Tooth");
        foreach (GameObject t in teeth)
        {
            Destroy(t);
        }

        // ⭐ показываем endGame UI
        if (endGameUI != null)
            endGameUI.SetActive(true);

        // ⭐ результат
        if (win)
            resultText.text = "You Won!";
        else
            resultText.text = "You Lose";

        // ⭐ статистика
        float playedTime = startTime - Mathf.Max(time, 0f);

        if (statisticText != null)
        {
            statisticText.text =
                "You've collected " +
                player.tooth +
                " tooth in " +
                playedTime.ToString("0") +
                " seconds";
        }

        // ⭐ останавливаем игру
        Time.timeScale = 0f;
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        float t = Mathf.Max(time, 0f);

        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void UpdateToothGoalUI()
    {
        if (toothGoalText == null || player == null) return;

        toothGoalText.text = player.tooth + " / " + toothGoal;
    }

}