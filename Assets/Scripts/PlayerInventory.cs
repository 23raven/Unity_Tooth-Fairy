using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int money = 0;
    public int tooth = 0;

    public int toothValue = 100;
    public int maxMoney = 1000; // ⭐ максимум денег

    private bool alreadyCollected = false;

    // Добавить деньги
    public void AddMoney(int amount)
    {
        money += amount;
        ClampMoney();
    }

    // Добавить зубы
    public void AddTooth(int amount)
    {
        tooth += amount;
        money += amount * toothValue;
        ClampMoney();
    }

    // Потратить деньги
    public bool SpendMoney(int amount)
    {
        if (money < amount) return false;

        money -= amount;
        return true;
    }

    // ⭐ Ограничение денег
    void ClampMoney()
    {
        if (money > maxMoney)
            money = maxMoney;

        if (money < 0)
            money = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyCollected) return; // ⭐ защита от двойного вызова

        Debug.Log("Player вошёл в trigger: " + other.name);

        if (other.CompareTag("Money"))
        {
            Debug.Log("Обнаружен объект с тегом Money");

            FillMoney();

            LevelRules rules = FindObjectOfType<LevelRules>();
            if (rules != null)
            {
                rules.PlayerEnteredBase();
            }

            }
        }

    public void FillMoney()
    {
        Debug.Log("Деньги ДО: " + money);

        money = maxMoney;

        Debug.Log("Деньги теперь MAX = " + money);
    }

    void ResetPickup()
    {
        alreadyCollected = false;
    }

}