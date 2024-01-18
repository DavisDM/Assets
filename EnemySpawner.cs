using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 2.0f; // Time in seconds between spawns
    private float nextSpawnTime = 0.0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    

    void SpawnEnemy()
    {
        Vector2 spawnPosition = GetSpawnPositionOutsideCameraView();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector2 GetSpawnPositionOutsideCameraView()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        float x = 0f, y = 0f;
        if (Random.value > 0.5f) // Randomly choose whether to spawn horizontally or vertically
        {
            x = Random.value > 0.5f ? cam.transform.position.x + width / 2 + 1 : cam.transform.position.x - width / 2 - 1;
            y = Random.Range(cam.transform.position.y - height / 2, cam.transform.position.y + height / 2);
        }
        else
        {
            y = Random.value > 0.5f ? cam.transform.position.y + height / 2 + 1 : cam.transform.position.y - height / 2 - 1;
            x = Random.Range(cam.transform.position.x - width / 2, cam.transform.position.x + width / 2);
        }

        return new Vector2(x, y);
    }
}
