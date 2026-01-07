using UnityEngine;

public sealed class Killable : MonoBehaviour
{
    [SerializeField] float scoreOnKill = 50f;
    public float ScoreOnKill => scoreOnKill;

    [SerializeField] float destroyDelayOverride = -1f; 

    bool dead;

    public void Die()
    {
        if (dead) return;
        dead = true;

        EnemyAnimDriver ad = GetComponent<EnemyAnimDriver>();
        if (ad == null) ad = GetComponentInParent<EnemyAnimDriver>();

        if (ad != null) ad.PlayDeath();

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        
        EnemyShooter es = GetComponent<EnemyShooter>();
        if (es != null) es.enabled = false;

        FlyingEnemy fe = GetComponent<FlyingEnemy>();
        if (fe != null) fe.enabled = false;

        float delay = 0.6f;
        if (destroyDelayOverride > 0f) delay = destroyDelayOverride;
        else if (ad != null) delay = Mathf.Max(0.1f, ad.DeathClipLength);

        Destroy(gameObject, delay);
    }
}
