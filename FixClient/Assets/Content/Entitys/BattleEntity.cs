using System.Collections.Generic;

namespace FixSystem
{
    public class BattleEntity : Entity
    {
        public FixVector2 target;
        public BattleEntity(World world) : base(world)
        {

        }
        public override void Init()
        {
            tranform = new FixTransform(FixVector2.Zero);
            trigger = new CircularTrigger(Fix64.One, this);
            trigger.OnTrigger += (item) =>
            {
                System.Console.WriteLine($"{ID}碰撞到{item.ID}");
            };
            target = tranform.position;
        }

        public override void Update(Fix64 deltaTime)
        {
            tranform.MoveTo(target, deltaTime * 5);
        }

        public void SetPosition(float x, float y)
        {
            tranform.position.x = (Fix64)x;
            tranform.position.y = (Fix64)y;
            target = tranform.position;
        }

        public void SetTarget(float x, float y)
        {
            target.x = (Fix64)x;
            target.y = (Fix64)y;
        }
    }
}