using System.Collections.Generic;
using TrueSync;


namespace FixSystem
{

    /*
        将所有需要进行碰撞检测的碰撞器加入到一个物理世界
        在物理世界的Update进行碰撞周期的管理
        -- 管理所有的碰撞检测
        -- 添加碰撞物体
        -- 移除碰撞物体
        -- 对四叉树的实现

        四叉树的一些参数:
        -- 管理范围:
            对于固定大小的场景,且场景尺寸不大情况,则管理范围就是场景的整个大小
            对于非固定大小的场景,或者场景尺寸很大的情况,则需要对场景进行九宫划分,此时管理范围就是一个九宫的大小(TODO,待验证)
        -- 最大层数:
            层数划分越多,则每个叶节点的范围就越小,若碰撞器大于叶节点的范围,则失去了划分四叉树的意义
            故:最大层数划分后的叶节点的范围 : 每个节点的最大碰撞器数量 * 碰撞器平均大小     (TODO,待验证)
        -- 每个节点的碰撞器数量:
            每个节点的碰撞器数量越大: 则每个循环列表的数量就越多,则每次循环的数量的就越大,可能以10个最佳?   (TODO,待验证)
    */


    [System.Serializable]
    public class PhysicsWorld
    {
        /// <summary>
        /// 待检测的碰撞列表
        /// </summary>
        public List<BaseCollider> colliderChectList = new List<BaseCollider>();
        /// <summary>
        /// 当前帧要移除的碰撞器
        /// </summary>
        public List<BaseCollider> removeColliderList = new List<BaseCollider>();
        /// <summary>
        /// 当前帧检测的碰撞器组列表
        /// </summary>
        public List<ColliderGroup> colliderDatas = new List<ColliderGroup>();
        /// <summary>
        /// 碰撞检测的四叉树,在每一帧开始把当前帧的所有碰撞器依次插入
        /// </summary>
        public QuadNode tree;

        /// <summary>
        /// 四叉树的范围
        /// </summary>
        /// <param name="bound"></param>
        public PhysicsWorld(TSVector2 size)
        {
            tree = new QuadNode(new Rectangle(TSVector2.zero, size));
        }


        public void AddCollider(params BaseCollider[] colliders)
        {
            foreach (var item in colliders)
            {
                item.world = this;
            }
            this.colliderChectList.AddRange(colliders);
        }

        public void RemoveCollider(BaseCollider collider)
        {
            removeColliderList.Add(collider);
        }


        /// <summary>
        /// 每帧执行遍历一次碰撞检测
        /// -- 将需要移除的碰撞器从检测列表删除,并清空
        /// -- 清空树
        /// -- 将检测列表中的碰撞器都加入到四叉树中
        /// -- 对每个节点进行碰撞检测(相互碰撞,独立碰撞)
        /// -- 刷新所有碰撞器的碰撞信息
        /// </summary>
        public void Update()
        {
            foreach (var item in removeColliderList)
            {
                colliderChectList.Remove(item);
            }
            removeColliderList.Clear();
            tree.Clear();
            foreach (var item in colliderChectList)
            {
                tree.Insert(item);
            }
            CheckCollider();

            foreach (var item in colliderChectList)
            {
                item.RefreshColliderInfo();
            }
        }

        /// <summary>
        /// 遍历整个树,获取所有节点
        /// 对每个节点进行相互碰撞检测和独立碰撞检测
        /// </summary>
        private void CheckCollider()
        {
            colliderDatas.Clear();
            List<QuadNode> findList = new List<QuadNode>();
            findList.Add(tree);
            while (findList.Count > 0)
            {
                var node = findList[0];
                findList.RemoveAt(0);
                if (node.nodes != null)
                {
                    findList.AddRange(node.nodes);
                }

                MutualCheck(node);
                AloneCheck(node);
            }

            foreach (var item in colliderDatas)
            {
                Collision(item.collider1, item.collider2);
            }
        }

        /// <summary>
        /// 相互碰撞:只有一个碰撞器列表,列表中所有碰撞器两两进行碰撞检测
        /// 适用于每个节点存储的碰撞器列表
        /// </summary>
        private void MutualCheck(QuadNode node)
        {
            var colliders = node.colliders;
            if (colliders.Count <= 0)
            {
                return;
            }
            // Debug.Log(node.name + "正在进行相互碰撞检测:" + colliders.Count);
            for (int i = 0; i < colliders.Count - 1; i++)
            {
                for (int j = i + 1; j < colliders.Count; j++)
                {
                    colliderDatas.Add(new ColliderGroup()
                    {
                        collider1 = colliders[i],
                        collider2 = colliders[j],
                    });
                }
            }
        }
        /// <summary>
        /// 独立碰撞:有两个碰撞器列表,一个列表的碰撞器只去跟另外一个列表的碰撞器进行碰撞检测
        /// 适用于每个节点存储的碰撞器列表 和 其所有父节点存储的碰撞器列表
        /// </summary>
        private void AloneCheck(QuadNode node)
        {
            var colliders = node.colliders;
            if (colliders.Count <= 0)
            {
                return;
            }
            List<BaseCollider> parentColliders = new List<BaseCollider>();
            var curParent = node.parent;
            while (curParent != null)
            {
                parentColliders.AddRange(curParent.colliders);
                curParent = curParent.parent;
            }
            if (parentColliders.Count <= 0)
            {
                return;
            }
            // Debug.Log(node.name + "正在进行独立碰撞检测:" + colliders.Count + "-" + parentColliders.Count);

            foreach (var collider1 in colliders)
            {
                foreach (var collider2 in parentColliders)
                {
                    colliderDatas.Add(new ColliderGroup()
                    {
                        collider1 = collider1,
                        collider2 = collider2
                    });
                }
            }
        }


        private void Collision(BaseCollider collider1, BaseCollider collider2)
        {
            if (PhysicsManager.IsOverlap(collider1, collider2))
            {
                collider1.Collision(collider2);
                collider2.Collision(collider1);
            }
        }
    }

    /// <summary>
    /// 碰撞器组
    /// </summary>
    [System.Serializable]
    public class ColliderGroup
    {
        public BaseCollider collider1;
        public BaseCollider collider2;
    }
}