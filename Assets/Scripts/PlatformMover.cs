using UnityEngine;

public sealed class PlatformMover : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.left * GameManager.Instance.CurrentSpeed * Time.deltaTime;

        if (transform.position.x < GameManager.Instance.DespawnX)
            Destroy(gameObject);
    }
}
