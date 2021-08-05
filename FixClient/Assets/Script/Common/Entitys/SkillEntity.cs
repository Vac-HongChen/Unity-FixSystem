using TrueSync;

namespace FixSystem
{
    /// <summary>
    /// 飞行类技能
    /// </summary>
    public class SkillEntity : Entity
    {
        public BattleEntity master { get; private set; }
        private ReleaseData releaseData;
        public FP speed { get; private set; } = FP.One * 10;
        public TSVector2 moveDir { get; private set; }
        public TSVector2 birthPoint { get; private set; }
        public FP maxDistance = 10;
        public SkillEntity(World world, BattleEntity master, ReleaseData releaseData) : base(world)
        {
            this.master = master;
            this.releaseData = releaseData;
            transform.position = master.transform.position;
            moveDir = VectorTools.AngleToVector(releaseData.angle);
            birthPoint = master.transform.position;
        }

        public override void Init()
        {
            base.Init();
            BoxCollider collider = new BoxCollider(this);
            collider.size = TSVector2.one * 0.5f;
            this.collider = collider;
            this.collider.OnColliderEnter += OnColliderEnter;
        }

        public override void LogicUpdate(FP deltaTime)
        {
            base.LogicUpdate(deltaTime);
            transform.position += moveDir * speed * deltaTime;

            // 如果超越一定距离就直接销毁
            if (TSVector2.DistanceSquared(birthPoint, transform.position) >= maxDistance * maxDistance)
            {
                Destroy();
            }
        }

        private void OnColliderEnter(BaseCollider collider)
        {
            if (collider.entity is MonsterEntity)
            {
                // 造成伤害
                Destroy();
            }
        }
    }
}