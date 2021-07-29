using System;

namespace FixSystem
{
    public abstract class BaseTrigger
    {
        public Entity entity { get; private set; }
        public event Action<Entity> OnTrigger;

        /// <summary>
        /// 检测是否跟该物体发生碰撞TODO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract bool IsTrigger(Entity entity);
        
        public BaseTrigger(Entity entity)
        {
            this.entity = entity;
        }

        /// <summary>
        /// 产生碰撞
        /// </summary>
        public void Trigger(Entity entity)
        {
            OnTrigger?.Invoke(entity);
        }
    }
}