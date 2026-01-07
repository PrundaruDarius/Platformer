using UnityEngine;

public class BackgroundFade : MonoBehaviour
{
    public SpriteRenderer bg1;
    public SpriteRenderer bg2;

    [Header("Transition Settings")]
    public float transitionDuration = 5f; 

    float timer = 0f;
    bool forward = true;

    void Start()
    {
        SetAlpha(bg1, 1f);
        SetAlpha(bg2, 0f);
    }

    void Update()
    {
        timer += Time.deltaTime / transitionDuration;

        if (forward)
        {
            SetAlpha(bg1, 1f - timer);
            SetAlpha(bg2, timer);
        }
        else
        {
            SetAlpha(bg1, timer);
            SetAlpha(bg2, 1f - timer);
        }

        if (timer >= 1f)
        {
            timer = 0f;
            forward = !forward;
        }
    }

    void SetAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = Mathf.Clamp01(alpha);
        sr.color = c;
    }
}
