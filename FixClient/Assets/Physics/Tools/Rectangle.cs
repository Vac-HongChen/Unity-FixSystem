using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// AABB包围框,矩形包围框
/// 垂直于世界坐标系
/// </summary>
public struct Rectangle
{
    /// <summary>
    /// 中心点
    /// </summary>
    public Vector2 center { get; private set; }
    /// <summary>
    /// 边界框的范围,是整个边界的一半大小
    /// </summary>
    public Vector2 extents { get; private set; }
    public Vector2 size { get; private set; }

    // 四个顶点
    public Vector2 leftDown { get; private set; }
    public Vector2 rightDown { get; private set; }
    public Vector2 rightTop { get; private set; }
    public Vector2 leftTop { get; private set; }

    // 四个极值
    public float minX { get; private set; }
    public float minY { get; private set; }
    public float maxX { get; private set; }
    public float maxY { get; private set; }


    /// <summary>
    /// 初始化
    /// </summary>
    public Rectangle(Vector2 center, Vector2 extents)
    {
        this.center = center;
        this.extents = extents;
        this.size = extents * 2;

        leftDown = center - extents;
        rightDown = center + new Vector2(extents.x, -extents.y);
        rightTop = center + extents;
        leftTop = center + new Vector2(-extents.x, extents.y);

        minX = (center - extents).x;
        minY = (center - extents).y;
        maxX = (center + extents).x;
        maxY = (center + extents).y;
    }

    /// <summary>
    /// 根据一系列顶点获取一个边界框,顶点数量需要大于两个
    /// </summary>
    public Rectangle(List<Vector2> vertexs)
    {
        if (vertexs.Count < 3)
        {
            throw new System.Exception("顶点数量少于三个,不能构成矩形边界框");
        }

        minX = vertexs[0].x;
        maxX = vertexs[0].x;
        minY = vertexs[0].y;
        maxY = vertexs[0].y;

        foreach (var item in vertexs)
        {
            if (item.x < minX)
            {
                minX = item.x;
            }
            if (item.y < minY)
            {
                minY = item.y;
            }
            if (item.x > maxX)
            {
                maxX = item.x;
            }
            if (item.y > maxY)
            {
                maxY = item.y;
            }
        }

        size = new Vector2(maxX - minX, maxY - minY);
        center = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
        extents = size / 2;

        leftDown = center - extents;
        rightDown = center + new Vector2(extents.x, -extents.y);
        rightTop = center + extents;
        leftTop = center + new Vector2(-extents.x, extents.y);
    }



    public override string ToString()
    {
        return $"center:{center},size:{size}";
    }
}