using System.Diagnostics;
using System.Collections.Generic;
using TrueSync;
using Customfloat = TrueSync.FP;
using CustomVector2 = TrueSync.TSVector2;
using Debug = BattleDebug;

namespace FixSystem
{
    /*
        所有碰撞检测采用的都是分离轴算法
        文档链接:
        https://blog.csdn.net/qq_36812406/article/details/82878380
        https://blog.csdn.net/qq_37043683/article/details/80375691
        https://www.cnblogs.com/sevenyuan/p/7125642.html

        简述:
        分离轴定理是一项用于检测凸多边形碰撞的技术
        假设拿一束光源从不同角度照射两个图形上,则会一系列阴影出现,如果在某个角度下两个图形阴影有间隙的情况出现,则说明这两个图形没有碰撞,否则一定有接触
        -- 从编程的角度来看,如果每个角度都检测一边,自然能够完成任务,但是性能却是有所不足.
        -- 事实上,由于多边形的性质,需要检测其中几个关键的角度即可.
        -- 在这些角度下观察,则会形成一条投影轴,观察两个图形在该投影轴上的投影是否重叠即可

        关键的投影轴:
        -- 多边形和多边形:
            将两个多边形的每条边的法向量都看做投影轴,投影轴数量为两个多边形的边数之和
        -- 圆形和圆形:
            两个圆心的连接向量看做投影轴    1
        -- 圆形和多边形:
            多边形每条边的法向量 + 距离圆形最近的顶点到圆心的向量   n+1

        各种图形在投影轴的投影区间:
        -- 多边形
            1.计算每个顶点在投影轴的上的长度
            2.取出最小长度和最大长度作为该多边形在该投影轴上的投影区间
        -- 圆形
            1.计算圆心在投影轴的上的长度
            2.将该长度+-半径,作为该圆形在投影轴的投影区间

        投影区间的关系比较:
        1.一个图形的最大投影 小于 另外图形的最小投影
        2.一个图形的最小投影 大于 另外图形的最大投影

        TODO:
            支持更多形状的碰撞检测:
            扇形,射线

        注意:
        分离轴算法只适用于凸多边形,凹多边形则不适用这种方法,需要将凹多边形拆解为凸多边形
        分离轴算法无法获取碰撞点的位置
    */
    public static class PhysicsManager
    {
        /// <summary>
        /// 判断两个碰撞器是否产生碰撞
        /// </summary>
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
            if (shape1 is CircularShape && shape2 is OBBShape)
            {
                return IsOverlap((CircularShape)shape1, (OBBShape)shape2);
            }
            if (shape1 is OBBShape && shape2 is CircularShape)
            {
                return IsOverlap((CircularShape)shape2, (OBBShape)shape1);
            }

            Debug.Log($"没有这两种形状的碰撞逻辑:{shape1.GetType()}-{shape1.GetType()}");
            return false;
        }


        public static bool IsOverlap(CircularShape shape1, OBBShape shape2)
        {
            var index = 0;
            var dis = CustomVector2.SqrMagnitude(shape2.vertexs[0] - shape1.center);
            for (int i = 1; i < shape2.vertexs.Count; i++)
            {
                var temp = CustomVector2.SqrMagnitude(shape2.vertexs[i] - shape1.center);
                if (temp < dis)
                {
                    dis = temp;
                    index = i;
                }
            }
            Customfloat aMin, aMax, bMin, bMax;
            var axis = shape1.center - shape2.vertexs[index];
            ComputeProjective(shape1, axis, out aMin, out aMax);
            ComputeProjective(shape2, axis, out bMin, out bMax);
            if (aMax < bMin || bMax < aMin)
                return false;

            foreach (var item in shape2.projections)
            {
                ComputeProjective(shape1, item, out aMin, out aMax);
                ComputeProjective(shape2, item, out bMin, out bMax);
                if (aMax < bMin || bMax < aMin)
                    return false;
            }
            return true;
        }

        public static bool IsOverlap(CircularShape shape1, CircularShape shape2)
        {
            Customfloat aMin, aMax, bMin, bMax;
            var axis = shape1.center - shape2.center;
            ComputeProjective(shape1, axis, out aMin, out aMax);
            ComputeProjective(shape2, axis, out bMin, out bMax);
            if (aMax < bMin || bMax < aMin)
                return false;
            return true;
        }

        public static bool IsOverlap(OBBShape shape1, OBBShape shape2)
        {
            Customfloat aMin, aMax, bMin, bMax;
            // 遍历a所有的边向量,将当前边向量视为目标直线,对该直线求投影
            // 每个顶点在该直线的投影长度为:顶点向量 点乘 投影轴
            // 判断两个多边形的投影向量是否重叠,则是判断两个多边形的最小投影长度 最大投影长度的关系
            // 如果一个多边形的最大投影长度 小于 另一个多边形的最小投影长度,则说明没有重叠
            foreach (var edgeVecto in shape1.projections)
            {
                ComputeProjective(shape1, edgeVecto, out aMin, out aMax);
                ComputeProjective(shape2, edgeVecto, out bMin, out bMax);
                // Debug.Log($"aMin:{aMin}-aMax:{aMax}-bMin:{bMin}-bMax:{bMax}");

                if (aMax < bMin || bMax < aMin)
                    return false;
            }
            foreach (var edgeVecto in shape2.projections)
            {
                ComputeProjective(shape1, edgeVecto, out aMin, out aMax);
                ComputeProjective(shape2, edgeVecto, out bMin, out bMax);
                // Debug.Log($"aMin:{aMin}-aMax:{aMax}-bMin:{bMin}-bMax:{bMax}");

                if (aMax < bMin || bMax < aMin)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// 计算圆形的投影区间
        /// </summary>
        private static void ComputeProjective(CircularShape shape, CustomVector2 axis, out Customfloat min, out Customfloat max)
        {
            var value = CustomVector2.Dot(shape.center, axis.normalized);
            min = value - shape.radius;
            max = value + shape.radius;
        }

        /// <summary>
        /// 计算多边形的投影区间
        /// </summary>
        private static void ComputeProjective(OBBShape shape, CustomVector2 axis, out Customfloat min, out Customfloat max)
        {
            var vertexs = shape.vertexs;
            List<Customfloat> list = new List<Customfloat>();
            foreach (var item in vertexs)
            {
                // a 点乘 b / b的长度
                list.Add(CustomVector2.Dot(item, axis.normalized));
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
}