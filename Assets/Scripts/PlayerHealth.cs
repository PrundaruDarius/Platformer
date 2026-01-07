using UnityEngine;

public sealed class PlayerHealth : MonoBehaviour
{
    bool dead;

    public void Die()
    {
        if (dead) return;
        dead = true;

        PlayerAnimator pa = GetComponent<PlayerAnimator>();
        if (pa == null) pa = GetComponentInParent<PlayerAnimator>();
        if (pa == null) pa = GetComponentInChildren<PlayerAnimator>(true);

        if (pa != null)
            pa.PlayDeath();

        if (GameManager.Instance != null && !GameManager.Instance.IsGameOver)
            GameManager.Instance.GameOver();
    }
}
