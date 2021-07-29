namespace FixSystem
{
    public class PlayerEntity : BattleEntity
    {
        public FrameInput input = new FrameInput();
        public override void Init()
        {
            base.Init();
        }
        public PlayerEntity(World world) : base(world)
        {

        }

        public override void Update(Fix64 deltaTime)
        {
            tranform.MoveTo(input.clickPostion, deltaTime * 5);
        }
    }
}