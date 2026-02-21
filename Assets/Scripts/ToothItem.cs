using UnityEngine;

public class ToothItem : MonoBehaviour
{
    public ToothSpawner spawner;
    public GameObject spawnPoint;

    public int price = 100;

    private bool picked = false; // ⭐ оставляем только ОДИН флаг

    void OnTriggerEnter2D(Collider2D other)
    {
        if (picked) return; // ⭐ САМАЯ ПЕРВАЯ СТРОКА

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null) return;

        Debug.Log("Player вошёл в Tooth");

        if (inventory.money < price)
        {
            Debug.Log("Недостаточно денег — Tooth НЕ уничтожается");
            return;
        }

        picked = true; // ⭐ блокируем ВСЁ сразу

        inventory.money -= price;
        inventory.tooth += 1;

        Debug.Log("Tooth куплен");

        if (spawner != null)
            spawner.OnToothDestroyed(spawnPoint);

        Destroy(gameObject);
    }
}