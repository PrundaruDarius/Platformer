using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 14f;
    [SerializeField] float lifeTime = 2.5f;

    [Header("Back-shot feel")]
    [SerializeField] float backSpeedBonus = 3f;              
    [SerializeField] float backScrollCompensation = 1.0f;    

    [Header("Scoring")]
    [SerializeField] float enemyKillScore = 50f;

    Vector2 dir;
    bool fromPlayer;
    Transform ownerRoot;

    Collider2D myCol;

    public void Init(Vector2 direction, bool shotByPlayer, Transform owner)
    {
        dir = direction.normalized;
        fromPlayer = shotByPlayer;
        ownerRoot = owner;

        myCol = GetComponent<Collider2D>();

        
        if (myCol != null && ownerRoot != null)
        {
            var cols = ownerRoot.GetComponentsInChildren<Collider2D>(true);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i] != null)
                    Physics2D.IgnoreCollision(myCol, cols[i], true);
            }
        }

        
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, ang + 90f);

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        float scroll = (GameManager.Instance != null) ? GameManager.Instance.CurrentSpeed : 0f;

        Vector2 v = dir * speed;

        
        if (dir.x < 0f)
        {
            v += Vector2.left * (scroll * backScrollCompensation);
            v += Vector2.left * backSpeedBonus;
        }

        transform.position += (Vector3)(v * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        Bullet otherBullet = other.GetComponent<Bullet>();
        if (otherBullet != null)
        {
            Destroy(otherBullet.gameObject);
            Destroy(gameObject);
            return;
        }

        
        if (other.GetComponent<PlatformMover>() != null)
            return;

        
        if (fromPlayer && HasTagInParents(other, "Enemy"))
        {
            Killable k = other.GetComponentInParent<Killable>();
            if (k != null)
            {
                float score = k.ScoreOnKill;
                k.Die();

                if (GameManager.Instance != null)
                    GameManager.Instance.AddScore(score > 0f ? score : enemyKillScore);
            }

            Destroy(gameObject);
            return;
        }

        
        if (!fromPlayer && HasTagInParents(other, "Player"))
        {
            PlayerHealth ph = other.GetComponentInParent<PlayerHealth>();
            if (ph != null) ph.Die();

            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    static bool HasTagInParents(Collider2D c, string tag)
    {
        Transform t = c.transform;
        while (t != null)
        {
            if (t.CompareTag(tag)) return true;
            t = t.parent;
        }
        return false;
    }
}
