using TrueSync;

namespace FixSystem
{
    public class TSTransform
    {
        public TSVector2 position;
        public TSVector2 localScale;
        public FP angle;
        public TSTransform()
        {
            this.position = TSVector2.zero;
            this.localScale = TSVector2.one;
        }
        public TSTransform(TSVector2 position)
        {
            this.position = position;
            this.localScale = TSVector2.one;
        }


        /// <summary>
        /// 每一次逻辑帧执行一次
        /// 每一帧的移动距离 = 帧运行时间 * 移动速度
        /// </summary>
        /// <param name="target">目标位置</param>
        /// <param name="distance">每一帧的移动距离</param>
        public void MoveTo(TSVector2 target, FP distance)
        {
            if (TSVector2.DistanceSquared(target, position) <= distance * distance)
            {
                position = target;
                return;
            }
            var dir = target - position;
            dir.Normalize();
            position += dir * distance;
        }



        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var data = (TSTransform)obj;
            return data.position == this.position;
        }
    }
}