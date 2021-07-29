using UnityEngine;
using System.Collections.Generic;

public class BoxCollider : BaseCollider
{
    public Vector2 offset;
    public Vector2 size;
    public override IShape GetShape()
    {
        return new OBBShape(GetTrueVertexs());
    }
    public override Rectangle GetRectangle()
    {
        return new Rectangle(GetTrueVertexs());
    }



    /// <summary>
    /// 获取实际的顶点坐标
    /// </summary>
    /// <returns></returns>
    public List<Vector2> GetTrueVertexs()
    {
        List<Vector2> vertexs = new List<Vector2>();
        vertexs.Add(GetTransformationVector(new Vector2(-size.x / 2, -size.y / 2) + offset, LocalScale, Angle, Position));
        vertexs.Add(GetTransformationVector(new Vector2(-size.x / 2, size.y / 2) + offset, LocalScale, Angle, Position));
        vertexs.Add(GetTransformationVector(new Vector2(size.x / 2, size.y / 2) + offset, LocalScale, Angle, Position));
        vertexs.Add(GetTransformationVector(new Vector2(size.x / 2, -size.y / 2) + offset, LocalScale, Angle, Position));
        return vertexs;
    }

    /// <summary>
    /// 变换向量
    /// 1.计算缩放
    /// 2.计算旋转
    /// 3.计算偏移
    /// </summary>
    /// <param name="target">目标向量</param>
    /// <param name="localScale">缩放系数</param>
    /// <param name="angle">旋转角度</param>
    /// <param name="center">旋转中心</param>
    /// <returns></returns>
    private Vector2 GetTransformationVector(Vector2 target, Vector2 localScale, float angle, Vector2 center)
    {
        // 计算缩放后的坐标
        target = target * localScale;
        // 计算旋转后的坐标
        angle = angle / Mathf.Rad2Deg;
        var x = target.x * Mathf.Cos(angle) - target.y * Mathf.Sin(angle);
        var y = target.x * Mathf.Sin(angle) + target.y * Mathf.Cos(angle);
        // 计算偏移后的坐标
        return new Vector2(x, y) + center;
    }

    private void Reset()
    {
        offset = Vector2.zero;
        if (SpriteRenderer != null)
        {
            size = SpriteRenderer.size;
        }
        else
        {
            size = Vector2.one;
        }
    }
}