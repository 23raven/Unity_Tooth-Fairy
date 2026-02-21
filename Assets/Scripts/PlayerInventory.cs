using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    public int money = 0;
    public int tooth = 0;

    public int toothValue = 100;
    public int maxMoney = 1000;

    private bool moneyProcessing = false;

    private PlayerMovement movement;
    private Rigidbody2D rb;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // -------------------------
    // Ограничение денег
    // -------------------------
    void ClampMoney()
    {
        money = Mathf.Clamp(money, 0, maxMoney);
    }

    // -------------------------
    // Trigger вход
    // -------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player вошёл в trigger: " + other.name);

        if (other.CompareTag("Money"))
        {
            if (moneyProcessing) return;

            moneyProcessing = true;
            StartCoroutine(MoneyDelay());
        }
    }

    // -------------------------
    // Задержка для Money
    // -------------------------
    IEnumerator MoneyDelay()
    {
        Debug.Log("Money задержка началась");

        // ⭐ остановить управление
        if (movement != null)
            movement.enabled = false;

        // ⭐ полностью остановить физику
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // ⭐ здесь будет анимация
        yield return new WaitForSeconds(1f);

        FillMoney();

        LevelRules rules = FindObjectOfType<LevelRules>();
        if (rules != null)
            rules.PlayerEnteredBase();

        // ⭐ вернуть физику
        if (rb != null)
        {
            rb.simulated = true;
            rb.WakeUp();
        }

        // ⭐ вернуть управление
        if (movement != null)
            movement.enabled = true;

        moneyProcessing = false;
    }

    // -------------------------
    // Заполнение денег
    // -------------------------
    public void FillMoney()
    {
        Debug.Log("Деньги ДО: " + money);

        money = maxMoney;

        Debug.Log("Деньги теперь MAX = " + money);
    }
}