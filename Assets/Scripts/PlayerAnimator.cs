using UnityEngine;

public sealed class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerController2D controller;

    void Awake()
    {
        if (anim == null) anim = GetComponentInChildren<Animator>(true);
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (controller == null) controller = GetComponent<PlayerController2D>();

        
        if (anim != null) anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if (anim == null || rb == null || controller == null) return;

        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("Grounded", controller.IsGroundedPublic());
        anim.SetFloat("YVel", rb.linearVelocity.y);
    }

    public void PlayShoot()
    {
        if (anim == null) return;
        anim.ResetTrigger("Shoot");
        anim.SetTrigger("Shoot");
    }

    public void PlayDeath()
    {
        if (anim == null) return;
        anim.ResetTrigger("Death");
        anim.SetTrigger("Death");
    }
}
