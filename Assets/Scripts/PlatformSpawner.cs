using UnityEngine;

public sealed class PlatformSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject platformPrefab;

    [Header("Spawn")]
    [SerializeField] float spawnAheadX = 12f;

    [Header("Spacing (how far platforms are placed)")]
    [SerializeField] float minSpacing = 6f;
    [SerializeField] float maxSpacing = 9f;

    [Header("Spawn Rate (how often we add a new platform)")]
    [SerializeField] float minSpawnDistance = 9f;
    [SerializeField] float maxSpawnDistance = 13f;

    [Header("Height (random terrain)")]
    [SerializeField] float minY = -3f;
    [SerializeField] float maxY = 2f;

    [Header("Start")]
    [SerializeField] float prewarmDistance = 12f;

    [Header("Fairness")]
    [SerializeField] bool preventTwoEnemiesSamePlatform = true;
    [SerializeField] float minPlatformWidthForEnemies = 2.8f; 

    [Header("Shooter Enemy (on platform)")]
    [SerializeField] EnemyShooter enemyPrefab;
    [SerializeField, Range(0f, 1f)] float enemyChance = 0.25f;
    [SerializeField] float enemyHeightOffset = 0.7f;
    [SerializeField] int noEnemyFirstPlatforms = 2;

    [Header("Blocking Enemy (on platform start)")]
    [SerializeField] GameObject blockingEnemyPrefab;
    [SerializeField, Range(0f, 1f)] float blockingChance = 0.20f;
    [SerializeField] float blockingYOffset = 0.1f;
    [SerializeField] float blockingXInset = 0.25f;
    [SerializeField] int noBlockingFirstPlatforms = 5;

    float distanceCounter;
    float nextSpawnDistance;
    float nextSpawnX;
    int spawnedPlatforms;

    void Start()
    {
        distanceCounter = 0f;
        nextSpawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        nextSpawnX = Camera.main.transform.position.x + spawnAheadX;
        spawnedPlatforms = 0;

        float targetX = Camera.main.transform.position.x + spawnAheadX + prewarmDistance;
        while (nextSpawnX < targetX)
        {
            SpawnAt(nextSpawnX);
            nextSpawnX += Random.Range(minSpacing, maxSpacing);
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        distanceCounter += GameManager.Instance.CurrentSpeed * Time.deltaTime;

        if (distanceCounter >= nextSpawnDistance)
        {
            SpawnAt(nextSpawnX);

            nextSpawnX += Random.Range(minSpacing, maxSpacing);
            distanceCounter = 0f;
            nextSpawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        }
    }

    void SpawnAt(float x)
    {
        float y = Random.Range(minY, maxY);
        GameObject p = Instantiate(platformPrefab, new Vector3(x, y, 0f), Quaternion.identity);

        if (p.GetComponent<PlatformMover>() == null)
            p.AddComponent<PlatformMover>();

        
        float platformWidth = 999f;
        Collider2D c = p.GetComponent<Collider2D>();
        if (c != null) platformWidth = c.bounds.size.x;

        bool canSpawnEnemies = platformWidth >= minPlatformWidthForEnemies;

        bool spawnedBlocking = false;
        bool spawnedShooter = false;

        
        if (canSpawnEnemies && blockingEnemyPrefab != null && spawnedPlatforms >= noBlockingFirstPlatforms)
        {
            if (Random.value < blockingChance)
            {
                Bounds b = c != null ? c.bounds : new Bounds(p.transform.position, new Vector3(4f, 1f, 0f));

                float ex = b.min.x + blockingXInset;
                float ey = b.center.y + blockingYOffset;

                GameObject block = Instantiate(blockingEnemyPrefab, new Vector3(ex, ey, 0f), Quaternion.identity);
                block.transform.SetParent(p.transform, true);

                spawnedBlocking = true;
            }
        }

        
        if (canSpawnEnemies && enemyPrefab != null && spawnedPlatforms >= noEnemyFirstPlatforms)
        {
            bool allowed = true;
            if (preventTwoEnemiesSamePlatform && spawnedBlocking)
                allowed = false;

            if (allowed && Random.value < enemyChance)
            {
                Vector3 ePos = new Vector3(p.transform.position.x, p.transform.position.y + enemyHeightOffset, 0f);
                EnemyShooter e = Instantiate(enemyPrefab, ePos, Quaternion.identity);
                e.transform.SetParent(p.transform, true);

                spawnedShooter = true;
            }
        }

       

        spawnedPlatforms++;
    }
}
