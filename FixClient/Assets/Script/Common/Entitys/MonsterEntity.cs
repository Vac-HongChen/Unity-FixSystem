using TrueSync;
namespace FixSystem
{
    public class MonsterEntity : Entity
    {
        public MonsterEntity(World world) : base(world)
        {

        }
        public override void Init()
        {
            base.Init();
            BoxCollider collider = new BoxCollider(this);

            this.collider = collider;
            this.collider.OnColliderEnter += OnColliderEnter;
        }


        private void OnColliderEnter(BaseCollider collider)
        {
            // print(collider);
        }
    }
}