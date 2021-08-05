using System;
using TrueSync;

namespace FixSystem
{
    public class Entity
    {
        protected static Action<object> print = BattleDebug.Log;
        private static int EntityID = 0;
        public int ID { get; private set; }
        public World world { get; private set; }
        public TSTransform transform { get; private set; } = new TSTransform();
        public BaseCollider collider;
        /// <summary>
        /// 预定两种销毁方式(TODO)
        /// 1.由World统一管理,触发销毁
        /// 2.通过创建时委托销毁
        /// </summary>
        public event Action<Entity> OnDestroy;
        public Action<FP> OnLogicUpdate;

        public Entity(World world)
        {
            this.world = world;
            this.ID = ++EntityID;
            this.world.AddEntity(this);
            Init();
        }


        public virtual void Init()
        {

        }
        /// <summary>
        /// 逻辑帧
        /// </summary>
        public virtual void LogicUpdate(FP deltaTime)
        {
            OnLogicUpdate?.Invoke(deltaTime);
        }
        public virtual FrameState GetState(int frameId)
        {
            FrameState state = new FrameState(frameId);
            state.position = transform.position;
            return state;
        }
        /// <summary>
        /// 在当前帧结束时销毁物体
        /// </summary>
        public void Destroy()
        {
            world.RemoveEntity(this);
            OnDestroy?.Invoke(this);
        }
    }
}