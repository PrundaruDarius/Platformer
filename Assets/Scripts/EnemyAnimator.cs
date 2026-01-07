using UnityEngine;

public sealed class EnemyAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;

    void Awake()
    {
        if (anim == null) anim = GetComponentInChildren<Animator>(true);
    }

    public void PlayAttack()
    {
        if (anim == null) return;
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Attack");
    }

    public void PlayDeath()
    {
        if (anim == null) return;
        anim.ResetTrigger("Death");
        anim.SetTrigger("Death");
    }
}
