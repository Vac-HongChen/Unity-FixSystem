using System.Collections.Generic;
namespace FixSystem
{
    public class World
    {
        private List<Entity> entities = new List<Entity>();
        private HashSet<Entity> removeEntities = new HashSet<Entity>();

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }
        public void RemoveEntity(Entity entity)
        {
            if (entities.Contains(entity))
            {
                var state = removeEntities.Add(entity);
                if (state == false)
                {
                    System.Console.WriteLine("该物体正在被删除");
                }
            }
            else
            {
                System.Console.WriteLine("不存在该物体");
            }
        }

        public void TestRemove(Entity entity)
        {
            entities.Remove(entity);
        }

        public void Update(Fix64 deltaTime)
        {
            foreach (var item in entities)
            {
                item.Update(deltaTime);
            }
            // 每一帧执行一次
            // 遍历所有的触发器,判断是否与其他碰撞发生碰撞,如果碰撞则根据状态触发碰撞函数
            // TODO待优化
            foreach (var item in entities)
            {
                item.CheckTrigger(entities);
            }

            foreach (var item in removeEntities)
            {
                entities.Remove(item);
            }
        }
    }
}