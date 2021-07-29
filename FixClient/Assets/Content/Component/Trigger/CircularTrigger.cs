namespace FixSystem
{
    /// <summary>
    /// 圆形碰撞器
    /// </summary>
    public class CircularTrigger : BaseTrigger
    {
        public Fix64 radius;
        public CircularTrigger(Fix64 radius, Entity entity) : base(entity)
        {
            this.radius = radius;
        }
        public override bool IsTrigger(Entity entity)
        {
            if (FixVector2.SqrMagnitude(this.entity.tranform.position - entity.tranform.position) <= radius * radius)
            {
                return true;
            }
            return false;
        }
    }
}