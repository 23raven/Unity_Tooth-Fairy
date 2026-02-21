using UnityEngine;
using System.Collections.Generic;

public class ToothSpawner : MonoBehaviour
{
    public GameObject toothPrefab;
    public int maxSpawns = 2;
    private GameObject lastSpawnPoint;

    private List<GameObject> spawnPoints = new List<GameObject>();
    private List<GameObject> occupiedPoints = new List<GameObject>();

    void Start()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("ToothSpawnPoint");

        foreach (GameObject p in points)
            spawnPoints.Add(p);

        Debug.Log("Spawn points: " + spawnPoints.Count);

        // стартовый спавн
        for (int i = 0; i < maxSpawns; i++)
            Spawn();
    }

    void Spawn()
    {
        List<GameObject> freePoints = spawnPoints.FindAll(p => !occupiedPoints.Contains(p));

        // ⭐ убираем последний использованный спавн, если есть другие варианты
        if (freePoints.Count > 1 && lastSpawnPoint != null)
        {
            freePoints.Remove(lastSpawnPoint);
        }

        if (freePoints.Count == 0) return;

        GameObject point = freePoints[Random.Range(0, freePoints.Count)];

        GameObject tooth = Instantiate(toothPrefab, point.transform.position, Quaternion.identity);

        ToothItem item = tooth.GetComponent<ToothItem>();
        item.spawner = this;
        item.spawnPoint = point;

        occupiedPoints.Add(point);
        lastSpawnPoint = point; // ⭐ запоминаем

        Debug.Log("Spawned Tooth at " + point.name);
    }

    // ⭐ вызывается когда Tooth уничтожен
    public void OnToothDestroyed(GameObject point)
    {
        Debug.Log("Tooth destroyed, freeing point: " + point.name);

        occupiedPoints.Remove(point);

        Spawn(); // сразу спавним новый
    }
}