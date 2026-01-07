using System.Collections.Generic;
using UnityEngine;

public sealed class PlatformToken : MonoBehaviour
{
    public bool hasTriggeredSpawn;

    static readonly List<PlatformToken> alive = new List<PlatformToken>();

    void OnEnable()
    {
        hasTriggeredSpawn = false;
        alive.Add(this);
    }

    void OnDisable()
    {
        alive.Remove(this);
    }

    public static float GetRightmostX()
    {
        float max = float.NegativeInfinity;

        for (int i = 0; i < alive.Count; i++)
        {
            if (alive[i] == null) continue;
            float x = alive[i].transform.position.x;
            if (x > max) max = x;
        }

        return max;
    }
}
