using UnityEngine;

public class ToothItem : MonoBehaviour
{
    public ToothSpawner spawner;
    public GameObject spawnPoint;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnToothDestroyed(spawnPoint);
        }
    }
}