using UnityEngine;

public sealed class EnemyKillable : MonoBehaviour
{
    [SerializeField] float scoreOnKill = 50f;
    public float ScoreOnKill => scoreOnKill;

    [SerializeField] float deathAnimTime = 0.6f; 

    bool dead;

    public void Die()
    {
        if (dead) return;
        dead = true;

        EnemyAnimator ea = GetComponent<EnemyAnimator>();
        if (ea == null) ea = GetComponentInParent<EnemyAnimator>();
        if (ea != null) ea.PlayDeath();

        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        
        EnemyShooter shooter = GetComponent<EnemyShooter>();
        if (shooter != null) shooter.enabled = false;

        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        
        Destroy(gameObject, deathAnimTime);
    }
}
