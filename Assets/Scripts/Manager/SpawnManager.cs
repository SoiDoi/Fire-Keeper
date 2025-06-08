using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public float spawnRadius = 12f;
    public float spawnInterval;

    [Header("References")]
    public Transform fireTransform;
    public static SpawnManager instance;
    public Transform spawnParent;
    public TimeManager timeManager;
    private bool isSpawning = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            if (timeManager.currentDay <= 10)
            {
                spawnInterval = 10f;
            }
            else if (timeManager.currentDay <= 30)
            {
                spawnInterval = 8f;
            }
            else if (timeManager.currentDay <= 90)
            {
                spawnInterval = 5f;
            }
            else
            {
                spawnInterval = 1f;
            }
            StartCoroutine(SpawnEnemiesRoutine());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (isSpawning)
        {
                SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = fireTransform.position + new Vector3(dir.x, dir.y, 0) * spawnRadius;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.transform.SetParent(spawnParent);
    }
}

