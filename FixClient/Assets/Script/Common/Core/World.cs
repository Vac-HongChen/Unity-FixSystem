using System.Collections.Generic;
using System;
using TrueSync;

namespace FixSystem
{
    public class World
    {
        private List<Entity> entities = new List<Entity>();
        private HashSet<Entity> removeList = new HashSet<Entity>();
        private List<Entity> addList = new List<Entity>();
        public event Action<Entity> OnAddEntity;
        public event Action<Entity> OnRemoveEntity;
        public PlayerEntity player;
        public string name;


        public void AddEntity(Entity entity)
        {
            addList.Add(entity);
            OnAddEntity?.Invoke(entity);
        }
        public void RemoveEntity(Entity entity)
        {
            if (entities.Contains(entity))
            {
                var state = removeList.Add(entity);
                if (state == false)
                {
                    System.Console.WriteLine("该物体正在被删除");
                }
            }
            else
            {
                System.Console.WriteLine("不存在该物体");
            }
            OnRemoveEntity?.Invoke(entity);
        }



        /// <summary>
        /// 逻辑更新
        /// deltaTime:逻辑更新的时间间隔
        /// </summary>
        public void LogicUpdate(FP deltaTime)
        {
            foreach (var item in addList)
            {
                entities.Add(item);
            }
            addList.Clear();
            foreach (var item in entities)
            {
                item.LogicUpdate(deltaTime);
            }
            CheckCollider();
            foreach (var item in removeList)
            {
                entities.Remove(item);
            }
            removeList.Clear();
        }



        /// <summary>
        /// 1.收集当前场景的中所有携带碰撞器的物体
        /// 2.双层循环遍历是否发生碰撞
        /// </summary>
        private void CheckCollider()
        {
            List<Entity> colliderEntitys = new List<Entity>();
            colliderEntitys = entities.FindAll((item) =>
            {
                return item.collider != null;
            });

            for (int i = 0; i < colliderEntitys.Count - 1; i++)
            {
                for (int j = i + 1; j < colliderEntitys.Count; j++)
                {
                    var collider1 = colliderEntitys[i].collider;
                    var collider2 = colliderEntitys[j].collider;

                    if (PhysicsManager.IsOverlap(collider1, collider2))
                    {
                        collider1.Collision(collider2);
                        collider2.Collision(collider1);
                    }
                }
            }

            foreach (var item in colliderEntitys)
            {
                item.collider.RefreshColliderInfo();
            }
        }
    }
}