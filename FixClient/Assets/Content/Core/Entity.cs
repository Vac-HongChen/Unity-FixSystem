using System;
using System.Collections.Generic;
namespace FixSystem
{
    public class Entity
    {
        private static int EntityID = 0;
        public int ID { get; private set; }
        public World world { get; private set; }
        public BaseTrigger trigger = null;
        public FixTransform tranform;
        /// <summary>
        /// 该单位的帧状态,即所有组件的信息
        /// </summary>
        public FrameState state;

        public Entity(World world)
        {
            this.world = world;
            this.ID = ++EntityID;
            Init();
            this.world.AddEntity(this);
        }


        public virtual void Init()
        {
            state = new FrameState();
            state.transform = tranform;
            tranform = new FixTransform(FixVector2.Zero);
        }
        public virtual void Update(Fix64 deltaTime)
        {

        }


        /// <summary>
        /// 触发检测
        /// -- 如果自身没有触发器,则返回
        /// -- 遍历所有物体列表
        ///     1.如果是自身,跳过
        ///     2.如果对方没有触发器,跳过
        ///     3.如果自身
        /// </summary>
        public void CheckTrigger(List<Entity> entities)
        {
            if (trigger == null)
            {
                return;
            }
            foreach (var item in entities)
            {
                if (item == this)
                {
                    continue;
                }
                if (item.trigger == null)
                {
                    continue;
                }
                if (trigger.IsTrigger(item))
                {
                    trigger.Trigger(item);
                }
            }
        }



        /// <summary>
        /// 在当前帧结束时销毁物体
        /// </summary>
        public void Destroy()
        {
            this.world.RemoveEntity(this);
        }
    }
}