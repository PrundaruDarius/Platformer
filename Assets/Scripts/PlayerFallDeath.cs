using UnityEngine;

public sealed class PlayerFallDeath : MonoBehaviour
{
    [SerializeField] float deathY = -6f;
    PlayerHealth ph;

    void Awake()
    {
        ph = GetComponent<PlayerHealth>();
        if (ph == null) ph = GetComponentInParent<PlayerHealth>();
    }

    void Update()
    {
        if (transform.position.y < deathY)
        {
            if (ph != null) ph.Die();
            enabled = false;
        }
    }
}
