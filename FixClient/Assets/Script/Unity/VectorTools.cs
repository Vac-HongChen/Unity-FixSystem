using UnityEngine;
public static class VectorTools
{
    /// <summary>
    /// 角度转向量(已经归一化)
    /// </summary>
    public static Vector2 AngleToVector(int angle)
    {
        var rad = angle * Mathf.Deg2Rad;
        var y = Mathf.Sin(rad);     // 求出斜边为1时的对边  y
        var x = Mathf.Cos(rad);     // 求出斜边为1时的临边  x
        return new Vector2(x, y);
    }

    /// <summary>
    /// 向量转角度
    /// </summary>
    public static int VectorToAngle(Vector2 dir)
    {
        dir.Normalize();
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return (int)angle;
    }
}