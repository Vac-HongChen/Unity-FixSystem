
using System.Collections.Generic;
using TrueSync;


namespace FixSystem
{
    public class BoxCollider : BaseCollider
    {
        public TSVector2 offset;
        public TSVector2 size;


        /// <summary>
        /// 默认设置size为1
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public BoxCollider(Entity entity) : base(entity)
        {
            this.size = TSVector2.one;
            this.offset = TSVector2.zero;
        }
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
        public List<TSVector2> GetTrueVertexs()
        {
            List<TSVector2> vertexs = new List<TSVector2>();
            vertexs.Add(GetTransformationVector(new TSVector2(-size.x / 2, -size.y / 2) + offset, LocalScale, Angle, Position));
            vertexs.Add(GetTransformationVector(new TSVector2(-size.x / 2, size.y / 2) + offset, LocalScale, Angle, Position));
            vertexs.Add(GetTransformationVector(new TSVector2(size.x / 2, size.y / 2) + offset, LocalScale, Angle, Position));
            vertexs.Add(GetTransformationVector(new TSVector2(size.x / 2, -size.y / 2) + offset, LocalScale, Angle, Position));
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
        private TSVector2 GetTransformationVector(TSVector2 target, TSVector2 localScale, FP angle, TSVector2 center)
        {
            // 计算缩放后的坐标
            target.x = target.x * localScale.x;
            target.y = target.y * localScale.y;
            // 计算旋转后的坐标
            angle = angle / TSMath.Rad2Deg;
            var x = target.x * TSMath.Cos(angle) - target.y * TSMath.Sin(angle);
            var y = target.x * TSMath.Sin(angle) + target.y * TSMath.Cos(angle);
            // 计算偏移后的坐标
            return new TSVector2(x, y) + center;
        }
    }
}
