using System.Threading;
using TrueSync;
namespace FixSystem
{
    /// <summary>
    /// 圆形碰撞器
    /// </summary>
    public class CircularCollider : BaseCollider
    {
        public TSVector2 offset;
        public FP radius = 1;
        public CircularCollider(Entity entity) : base(entity)
        {
            this.radius = 1;
            this.offset = TSVector2.zero;
        }
        public override IShape GetShape()
        {
            return new CircularShape(GetTrueRadius(), GetTrueCenter());
        }

        public override Rectangle GetRectangle()
        {
            return new Rectangle(GetTrueCenter(), new TSVector2(GetTrueRadius(), GetTrueRadius()));
        }


        /// <summary>
        /// 获取实际半径
        /// 当前半径 * x,y的最大缩放系数
        /// </summary>
        private FP GetTrueRadius()
        {
            var x = TSMath.Abs(LocalScale.x);
            var y = TSMath.Abs(LocalScale.y);
            return radius * (x > y ? x : y);
        }
        /// <summary>
        /// 获取实际中心
        /// 当前位置 + 位置偏差
        /// </summary>
        /// <returns></returns>
        private TSVector2 GetTrueCenter()
        {
            return Position + offset;
        }
        private void Reset()
        {
            // offset = TSVector2.zero;
            // if (SpriteRenderer != null)
            // {
            //     var width = SpriteRenderer.size.x;
            //     var height = SpriteRenderer.size.y;
            //     radius = width > height ? width / 2 : height / 2;
            // }
            // else
            // {
            //     radius = 1;
            // }
        }
    }
}