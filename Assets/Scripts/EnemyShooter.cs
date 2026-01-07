using UnityEngine;

public sealed class EnemyShooter : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float reloadTime = 1.6f;

    float timer;

    void Start()
    {
        timer = Random.Range(0.2f, reloadTime);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Shoot();
            timer = reloadTime;
        }
    }

    void Shoot()
    {
        EnemyAnimator ea = GetComponent<EnemyAnimator>();
        if (ea != null) ea.PlayAttack();
        Bullet b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        b.Init(Vector2.left, false, transform);
    }
}
