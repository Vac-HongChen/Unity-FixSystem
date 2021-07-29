using System.Collections.Generic;
using UnityEngine;


/*
    处理所有形状的碰撞检测
    -- 旋转矩形 和 旋转矩形
    -- 圆形 和 圆形(TODO)
    -- 旋转矩形 和 圆形(TODO)
*/
public static class PhysicsManager
{

    /// <summary>
    /// 判断两个碰撞器是否产生碰撞
    /// </summary>
    /// <param name="collider1"></param>
    /// <param name="collider2"></param>
    /// <returns></returns>
    public static bool IsOverlap(BaseCollider collider1, BaseCollider collider2)
    {
        var shape1 = collider1.GetShape();
        var shape2 = collider2.GetShape();
        if (shape1 is OBBShape && shape2 is OBBShape)
        {
            return IsOverlap((OBBShape)shape1, (OBBShape)shape2);
        }
        if (shape1 is CircularShape && shape2 is CircularShape)
        {
            return IsOverlap((CircularShape)shape1, (CircularShape)shape2);
        }

        Debug.Log($"没有这两种形状的碰撞逻辑:{shape1.GetType()}-{shape1.GetType()}");
        return false;
    }







    /// <summary>
    /// 判断两个圆形是否产生重叠
    /// 圆心距离 < 半径之和 重叠
    /// 为了计算方便,统一求其平方
    /// </summary>
    public static bool IsOverlap(CircularShape shape1, CircularShape shape2)
    {
        var dis = Vector2.SqrMagnitude(shape1.center - shape2.center);
        var sum = (shape1.radius + shape2.radius) * (shape1.radius + shape2.radius);
        return dis < sum;
    }


    /// <summary>
    /// 判断两个OBB是否产生重叠(注意:只适应于凸多边形)
    /// 分离轴定理:https://www.cnblogs.com/sevenyuan/p/7125642.html
    /// 通过判断任意两个凸多边形在任意角度下的投影是否均存在重叠,来判断是否发生碰撞
    /// 若在某一角度下俩物体的投影存在间隙,则为不碰撞,否则发生碰撞
    /// 在程序中,遍历所有角度是不现实的,如何确定投影轴是一个问题.
    /// 其实投影轴的数量与多边形的边数相等即可
    /// 1.如何确定多边形的各个投影轴
    ///     -- 取每条边的法向量的单位向量(即n边形有n个投影轴)
    /// 2.如何将多边形投射到某条投影轴上
    ///     -- 多边形的每个顶点 点乘 投影轴
    /// 3.如何检测两段投影是否发生重叠
    ///     -- 取两个多边形的每个顶点在投影轴的最大长度和最小长度
    ///     -- 当一个多边形的最大长度 小于 另一个多边形的最小长度,则没有重叠
    /// </summary>
    public static bool IsOverlap(OBBShape shape1, OBBShape shape2)
    {
        float aMin, aMax, bMin, bMax;
        // 遍历a所有的边向量,将当前边向量视为目标直线,对该直线求投影
        // 每个顶点在该直线的投影长度为:顶点向量 点乘 投影轴
        // 判断两个多边形的投影向量是否重叠,则是判断两个多边形的最小投影长度 最大投影长度的关系
        // 如果一个多边形的最大投影长度 小于 另一个多边形的最小投影长度,则说明没有重叠
        foreach (var edgeVecto in shape1.projections)
        {
            ComputeProjective(shape1.vertexs, edgeVecto, out aMin, out aMax);
            ComputeProjective(shape2.vertexs, edgeVecto, out bMin, out bMax);
            // Debug.Log($"aMin:{aMin}-aMax:{aMax}-bMin:{bMin}-bMax:{bMax}");

            if (aMax < bMin || bMax < aMin)
                return false;
        }
        foreach (var edgeVecto in shape2.projections)
        {
            ComputeProjective(shape1.vertexs, edgeVecto, out aMin, out aMax);
            ComputeProjective(shape2.vertexs, edgeVecto, out bMin, out bMax);
            // Debug.Log($"aMin:{aMin}-aMax:{aMax}-bMin:{bMin}-bMax:{bMax}");

            if (aMax < bMin || bMax < aMin)
                return false;
        }
        return true;
    }



    /// <summary>
    /// 计算一组顶点向量在某向量(必须向量标准化)上的最大投影长度和最小投影长度
    /// </summary>
    private static void ComputeProjective(List<Vector2> vertexs, Vector2 axis, out float min, out float max)
    {
        List<float> list = new List<float>();
        foreach (var item in vertexs)
        {
            // a 点乘 b / b的长度
            list.Add(Vector2.Dot(item, axis));
        }
        // 将所有长度从小到大排列
        list.Sort((item1, item2) =>
        {
            if (item1 > item2)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        });
        min = list[0];
        max = list[list.Count - 1];
    }
}