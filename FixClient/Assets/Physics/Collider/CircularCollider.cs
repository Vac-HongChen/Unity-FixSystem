using System.Threading;
using UnityEngine;

/// <summary>
/// 圆形碰撞器
/// </summary>
public class CircularCollider : BaseCollider
{
    public Vector2 offset;
    public float radius = 1;

    public override IShape GetShape()
    {
        return new CircularShape(GetTrueRadius(), GetTrueCenter());
    }

    public override Rectangle GetRectangle()
    {
        return new Rectangle(GetTrueCenter(), new Vector2(GetTrueRadius(), GetTrueRadius()));
    }


    /// <summary>
    /// 获取实际半径
    /// 当前半径 * x,y的最大缩放系数
    /// </summary>
    private float GetTrueRadius()
    {
        var x = Mathf.Abs(LocalScale.x);
        var y = Mathf.Abs(LocalScale.y);
        return radius * (x > y ? x : y);
    }
    /// <summary>
    /// 获取实际中心
    /// 当前位置 + 位置偏差
    /// </summary>
    /// <returns></returns>
    private Vector2 GetTrueCenter()
    {
        return Position + offset;
    }
    private void Reset()
    {
        offset = Vector2.zero;
        if (SpriteRenderer != null)
        {
            var width = SpriteRenderer.size.x;
            var height = SpriteRenderer.size.y;
            radius = width > height ? width / 2 : height / 2;
        }
        else
        {
            radius = 1;
        }
    }
}