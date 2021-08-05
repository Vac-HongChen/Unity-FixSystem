using UnityEngine;
using TrueSync;

public static class FixHelper
{
    public static Vector2 ToVector2(this TSVector2 vector)
    {
        return new Vector2((float)vector.x, (float)vector.y);
    }
    public static TSVector2 ToFixVector2(this Vector2 vector)
    {
        return new TSVector2((FP)vector.x, (FP)vector.y);
    }
}