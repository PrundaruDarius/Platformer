using UnityEngine;

public sealed class FlyingEnemy : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform firePoint;

    [Header("Movement")]
    [SerializeField] float speedMultiplier = 0.55f;
    [SerializeField] float followSmooth = 6f;
    [SerializeField] float yOffset = 0.5f;

    [Header("Shooting")]
    [SerializeField] float reloadTime = 1.6f;
    [SerializeField] float minShootDistance = 30f;

    static int aliveCount = 0;
    public static int AliveCount => aliveCount;

    Transform player;
    float timer;
    EnemyAnimDriver animDriver;

    void OnEnable() => aliveCount++;

    void OnDestroy()
    {
        aliveCount--;
        if (aliveCount < 0) aliveCount = 0;
    }

    void Start()
    {
        animDriver = GetComponent<EnemyAnimDriver>();
        if (animDriver == null) animDriver = GetComponentInParent<EnemyAnimDriver>();
        if (animDriver == null) animDriver = GetComponentInChildren<EnemyAnimDriver>(true);

        FindPlayer();
        timer = Random.Range(0.1f, reloadTime);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        float baseSpeed = (GameManager.Instance != null ? GameManager.Instance.CurrentSpeed : 5f);
        float spd = baseSpeed * speedMultiplier;
        transform.position += Vector3.left * spd * Time.deltaTime;

        if (player == null) FindPlayer();

        if (player != null)
        {
            Vector3 pos = transform.position;
            float targetY = player.position.y + yOffset;
            pos.y = Mathf.Lerp(pos.y, targetY, followSmooth * Time.deltaTime);
            transform.position = pos;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TryShoot();
            timer = reloadTime;
        }

        if (GameManager.Instance != null && transform.position.x < GameManager.Instance.DespawnX)
            Destroy(gameObject);
    }

    void TryShoot()
    {
        if (player == null || bulletPrefab == null || firePoint == null) return;

        float dist = Vector2.Distance(player.position, firePoint.position);
        if (dist > minShootDistance) return;

        if (animDriver != null) animDriver.PlayShoot();

        Vector2 dir = ((Vector2)player.position - (Vector2)firePoint.position).normalized;
        Bullet b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        b.Init(dir, false, transform);
    }

    void FindPlayer()
    {
        PlayerHealth ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null) player = ph.transform;
    }
}
