using UnityEngine;

public sealed class PlayerShooter : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float reloadTime = 0.8f;
    [SerializeField] SpriteRenderer sr;

    float timer;
    PlayerAnimator pa;

    void Awake()
    {
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>(true);

        pa = GetComponent<PlayerAnimator>();
        if (pa == null) pa = GetComponentInParent<PlayerAnimator>();
        if (pa == null) pa = GetComponentInChildren<PlayerAnimator>(true);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        if (timer > 0f) timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && timer <= 0f)
        {
            if (bulletPrefab == null || firePoint == null)
            {
                timer = reloadTime;
                return;
            }

            Vector2 dir = Vector2.right;
            if (sr != null && sr.flipX) dir = Vector2.left;

            Bullet b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            b.Init(dir, true, transform);

            if (pa != null) pa.PlayShoot();

            timer = reloadTime;
        }
    }
}
