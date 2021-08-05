using UnityEngine;


/// <summary>
/// 射线
/// </summary>
public struct RayShape : IShape
{
    public Vector2 startPoint { get; private set; }
    public Vector2 endPoint { get; private set; }
    public float length { get; private set; }
    public Vector2 dir { get; private set; }


    /// <summary>
    /// 根据起点,终点进行初始化
    /// </summary>
    /// <param name="startPoint">起点</param>
    /// <param name="endPoint">终点</param>
    public RayShape(Vector2 startPoint, Vector2 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.length = Vector2.Distance(startPoint, endPoint);
        this.dir = (endPoint - startPoint).normalized;
    }


    /// <summary>
    /// 根据起点,方向,长度,进行初始化
    /// </summary>
    /// <param name="startPoint">起点</param>
    /// <param name="dir">方向,会自动设置为单位向量</param>
    /// <param name="length">长度</param>
    public RayShape(Vector2 startPoint, Vector2 dir, float length)
    {
        dir.Normalize();
        this.startPoint = startPoint;
        this.endPoint = startPoint * dir * length;
        this.length = length;
        this.dir = dir;
    }


    /// <summary>
    /// 是否包含某个点
    /// 判断起点到该点的单位方向是否等于该点到终点的单位方向
    /// 包含起点和终点
    /// </summary>
    public bool ContainPoint(Vector2 point)
    {
        if (point == startPoint || point == endPoint)
        {
            return true;
        }
        var dir1 = (point - startPoint).normalized;
        var dir2 = (endPoint - point).normalized;
        return dir1 == dir2;
    }

    public void Draw()
    {
        Gizmos.DrawLine(startPoint, endPoint);
    }
}