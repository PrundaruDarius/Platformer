using UnityEngine;

public sealed class EnemyAnimDriver : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator anim;

    [Header("Trigger Names")]
    [SerializeField] string shootTrigger = "Attack"; 
    [SerializeField] string deathTrigger = "Death";

    [Header("Options")]
    [SerializeField] bool useUnscaledTime = false;

    public float DeathClipLength { get; private set; } = 0.6f;

    void Awake()
    {
        if (anim == null) anim = GetComponentInChildren<Animator>(true);

        if (anim != null)
        {
            if (useUnscaledTime)
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;

            
            RuntimeAnimatorController c = anim.runtimeAnimatorController;
            if (c != null)
            {
                foreach (var clip in c.animationClips)
                {
                    if (clip != null && clip.name.ToLower().Contains("death"))
                    {
                        DeathClipLength = clip.length;
                        break;
                    }
                }
            }
        }
    }

    public void PlayShoot()
    {
        if (anim == null || string.IsNullOrEmpty(shootTrigger)) return;
        anim.ResetTrigger(shootTrigger);
        anim.SetTrigger(shootTrigger);
    }

    public void PlayDeath()
    {
        if (anim == null || string.IsNullOrEmpty(deathTrigger)) return;
        anim.ResetTrigger(deathTrigger);
        anim.SetTrigger(deathTrigger);
    }
}
