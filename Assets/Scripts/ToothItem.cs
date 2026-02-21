using UnityEngine;
using System.Collections;

public class ToothItem : MonoBehaviour
{
    public ToothSpawner spawner;
    public GameObject spawnPoint;

    public int price = 100;

    private bool picked = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (picked) return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null) return;

        Debug.Log("Player вошёл в Tooth");

        if (inventory.money < price)
        {
            Debug.Log("Недостаточно денег — Tooth НЕ уничтожается");
            return;
        }

        picked = true;

        StartCoroutine(CollectDelay(inventory));
    }

    IEnumerator CollectDelay(PlayerInventory inventory)
    {
        Debug.Log("Tooth задержка началась");

        PlayerMovement movement = inventory.GetComponent<PlayerMovement>();
        Rigidbody2D rb = inventory.GetComponent<Rigidbody2D>();

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

        // ⭐ покупка
        inventory.money -= price;
        inventory.tooth += 1;

        Debug.Log("Tooth куплен");

        if (spawner != null)
            spawner.OnToothDestroyed(spawnPoint);

        // ⭐ вернуть физику
        if (rb != null)
        {
            rb.simulated = true;
            rb.WakeUp();
        }

        // ⭐ вернуть управление
        if (movement != null)
            movement.enabled = true;

        Destroy(gameObject);
    }
}