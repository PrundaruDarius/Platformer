using UnityEngine;

public sealed class FlyingEnemySpawner : MonoBehaviour
{
    [SerializeField] FlyingEnemy flyingEnemyPrefab;

    [Header("Rules")]
    [SerializeField] float checkInterval = 10f;
    [SerializeField, Range(0f, 1f)] float spawnChance = 0.25f;
    [SerializeField] int maxAlive = 2;

    [Header("Spawn Area")]
    [SerializeField] float spawnX = 14f;
    [SerializeField] float minY = -1f;
    [SerializeField] float maxY = 3f;

    float timer;

    void Start()
    {
        timer = checkInterval;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = checkInterval;

        if (flyingEnemyPrefab == null) return;
        if (FlyingEnemy.AliveCount >= maxAlive) return;

        if (Random.value <= spawnChance)
        {
            float x = Camera.main.transform.position.x + spawnX;
            float y = Random.Range(minY, maxY);
            Instantiate(flyingEnemyPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        }
    }
}
