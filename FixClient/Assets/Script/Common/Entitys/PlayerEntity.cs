using System.Collections.Generic;
using TrueSync;

namespace FixSystem
{
    public class PlayerEntity : BattleEntity
    {
        public FP speed = FP.One * 5;
        public FrameOperation[] operations = new FrameOperation[] { };
        private TSVector2 target;
        public PlayerEntity(World world) : base(world)
        {

        }

        public override void Init()
        {
            target = transform.position;
            CircularCollider collider = new CircularCollider(this);
            this.collider = collider;
        }
        public override void LogicUpdate(FP deltaTime)
        {
            OnLogicUpdate?.Invoke(deltaTime);
            if (operations != null)
            {
                foreach (var item in operations)
                {
                    switch (item.keyCode)
                    {
                        case FrameOperation.KeyCode.LeftMouse:
                            target = item.mousePosition;
                            break;
                        case FrameOperation.KeyCode.Skill1:
                            ReleaseData data = new ReleaseData();
                            data.skillId = 1;
                            data.angle = VectorTools.VectorToAngle(item.mousePosition - transform.position);
                            SkillEntity skill = new SkillEntity(this.world, this, data);
                            break;
                    }
                }
                operations = null;
            }
            transform.MoveTo(target, deltaTime * speed);
        }
    }
}