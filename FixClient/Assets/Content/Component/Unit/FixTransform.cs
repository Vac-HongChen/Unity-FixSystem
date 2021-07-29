namespace FixSystem
{
    public class FixTransform
    {
        public FixVector2 position;
        public FixTransform(FixVector2 position)
        {
            this.position = position;
        }
        public void MoveTo(FixVector2 target, Fix64 speed)
        {
            if (FixVector2.SqrMagnitude(target - position) <= speed)
            {
                position = target;
                return;
            }
            var dir = target - position;
            dir.Normalize();
            position += dir * speed;
        }
    }
}