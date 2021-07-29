using System.Collections.Generic;
using UnityEngine;

/*
    四叉树节点

    通过四叉树的方式来实现:https://www.cnblogs.com/vana/p/10948789.html
    -- 利用四叉树思想分割场景中的所有碰撞器
    -- 为整个场景构建一个四叉树,将场景中所有的碰撞器插入到该四叉树中
    -- 插入规则:(从根节点开始)
            如果当前节点已经分裂(即已经创建了子节点),则向子节点中插入
            如果当前节点没有分裂,则插入到当前节点
                插入之后:
                -- 当前节点存储的碰撞器列表大于规定上限:进行分裂,并将所有碰撞器分发给分裂后的子节点
                -- 当前节点存储的碰撞器列表小于规定上限,没有操作
   
            分发规则:
                根据碰撞器在场景的位置进行分发,可以知道有以下三种情况:
                1.完全位于某个象限内部,则将碰撞器插入到该节点中
                2.完全位于某两个象限,则将碰撞器插入到这两个节点中
                3.位于四个象限,则将碰撞器插入到这四个节点中
                此时,某些位于边界的碰撞器则会在多个节点中存在

    时间复杂度:
        假设有100个物体,原始碰撞检测则需要两层循环(外层循环 0,98)(内层顺换 1,99)
        即:99 + 98 + ... + 1 = (99 + 1) * 99 / 2 = 4950
        时间复杂度为O(n^2)
        使用四叉树检测:
        这里只计算一种最优解的情况,只是用于简单的对比,实际情况只会更复杂,
        将四叉树分为两层,则叶节点的数量为16个,将100个碰撞器平均的存储到16个节点中,即每个节点最多为7个碰撞器
        再对每个节点进行遍历检测,则计算量为 (6 + 1) * 6 / 2 = 21;再计算所有区域,即21 * 16 = 336次

    BUG:由于分布规则产生的Bug,当两个碰撞器同时在边界上时,两个碰撞器会加入到两个区间,则会发生两个区间都去检测这两个碰撞器是否发生碰撞,重复判断

    修改:
        修改分裂规则:
        1.完全位于某个象限内部,则将碰撞器插入属于该象限的节点中
        2.否则就不插入,仍放在当前节点

        修改碰撞检测逻辑,将碰撞逻辑分为相互碰撞和独立碰撞
        1.相互碰撞:只有一个碰撞器列表,列表中所有碰撞器两两进行碰撞检测
            适用于每个节点存储的碰撞器列表
        2.独立碰撞:有两个碰撞器列表,一个列表的碰撞器只去跟另外一个列表的碰撞器进行碰撞检测
            适用于每个节点存储的碰撞器列表 和 其所有父节点存储的碰撞器列表
*/
[System.Serializable]
public class QuadNode
{
    public Rectangle bound { get; private set; }
    public QuadNode[] nodes { get; private set; }
    public List<BaseCollider> colliders;
    public int level { get; private set; }
    public QuadNode parent { get; private set; }
    public string name;
    private const int MAX_LEVELS = 3;
    private const int MAX_OBJECTS = 1;


    public QuadNode(Rectangle bound, int level = 0, QuadNode parent = null, int dir = 0)
    {
        this.bound = bound;
        this.level = level;
        this.parent = parent;
        colliders = new List<BaseCollider>();
        this.name = level + "-" + dir;
    }

    /// <summary>
    /// 分裂树
    /// </summary>
    private void Split()
    {
        var subWidth = bound.extents.x / 2;
        var subHeight = bound.extents.y / 2;

        var subSize = new Vector2(subWidth, subHeight);

        nodes = new QuadNode[4];
        nodes[0] = new QuadNode(new Rectangle(bound.center + subSize, subSize), level + 1, this, 0);
        nodes[1] = new QuadNode(new Rectangle(bound.center + new Vector2(-subSize.x, subSize.y), subSize), level + 1, this, 1);
        nodes[2] = new QuadNode(new Rectangle(bound.center - subSize, subSize), level + 1, this, 2);
        nodes[3] = new QuadNode(new Rectangle(bound.center - new Vector2(-subSize.x, +subSize.y), subSize), level + 1, this, 3);
    }


    /// <summary>
    /// 获取该碰撞器属于那个子节点,若都不属于则返回-1
    /// </summary>
    private int GetIndex(BaseCollider collider)
    {
        var position = collider.GetRectangle();

        var minX = position.minX;
        var minY = position.minY;
        var maxX = position.maxX;
        var maxY = position.maxY;

        if (minX > bound.center.x && minY > bound.center.y)
        {
            // 位于第一象限
            return 0;
        }

        if (maxX < bound.center.x && minY > bound.center.y)
        {
            // 位于第二象限
            return 1;
        }

        if (maxX < bound.center.x && maxY < bound.center.y)
        {
            // 位于第三象限
            return 2;
        }

        if (minX > bound.center.x && maxY < bound.center.y)
        {
            // 位于第四象限
            return 3;
        }
        return -1;
    }
    /// <summary>
    /// 插入到子节点(弃用)
    /// </summary>
    private void InsertChild(BaseCollider collider)
    {
        // 判断是在那个区域
        // 1.位于某个象限内
        // 2.位于某两个象限内(只可能是(1,2) (2,3) (3,4) (4,1))
        // 3.位于四个象限内

        var position = collider.GetRectangle();

        var minX = position.minX;
        var minY = position.minY;
        var maxX = position.maxX;
        var maxY = position.maxY;

        if (minX > bound.center.x && minY > bound.center.y)
        {
            // 位于第一象限
            nodes[0].Insert(collider);
            return;
        }


        if (maxX < bound.center.x && minY > bound.center.y)
        {
            // 位于第二象限
            nodes[1].Insert(collider);
            return;
        }

        if (maxX < bound.center.x && maxY < bound.center.y)
        {
            // 位于第三象限
            nodes[2].Insert(collider);
            return;
        }

        if (minX > bound.center.x && maxY < bound.center.y)
        {
            // 位于第四象限
            nodes[3].Insert(collider);
            return;
        }

        if (minY > bound.center.y)
        {
            // 位于12象限
            nodes[0].Insert(collider);
            nodes[1].Insert(collider);
            return;
        }

        if (maxY < bound.center.y)
        {
            // 位于34象限
            nodes[2].Insert(collider);
            nodes[3].Insert(collider);
            return;
        }

        if (minX > bound.center.x)
        {
            // 位于14象限
            nodes[0].Insert(collider);
            nodes[3].Insert(collider);
            return;
        }

        if (maxX < bound.center.x)
        {
            // 位于23象限
            nodes[1].Insert(collider);
            nodes[2].Insert(collider);
            return;
        }

        // 否则位于所有象限
        nodes[0].Insert(collider);
        nodes[1].Insert(collider);
        nodes[2].Insert(collider);
        nodes[3].Insert(collider);
    }


    /// <summary>
    /// 获取当前节点下的所有的叶节点
    /// </summary>
    public List<QuadNode> GetLeafNodes()
    {
        List<QuadNode> result = new List<QuadNode>();
        List<QuadNode> findList = new List<QuadNode>();
        findList.Add(this);
        while (findList.Count > 0)
        {
            var temp = findList[0];
            findList.RemoveAt(0);
            if (temp.nodes == null)
            {
                result.Add(temp);
            }
            else
            {
                findList.AddRange(temp.nodes);
            }
        }
        return result;
    }
    /// <summary>
    /// 获取当前节点下的所有节点
    /// </summary>
    public List<QuadNode> GetAllNodes()
    {
        List<QuadNode> result = new List<QuadNode>();
        List<QuadNode> findList = new List<QuadNode>();
        findList.Add(this);
        while (findList.Count > 0)
        {
            var temp = findList[0];
            result.Add(temp);
            findList.RemoveAt(0);

            if (temp.nodes != null)
            {
                findList.AddRange(temp.nodes);
            }
        }
        return result;
    }
    /// <summary>
    /// 获取整个四叉树的根节点
    /// </summary>
    public QuadNode GetRoot()
    {
        var node = this;
        while (true)
        {
            if (node.parent != null)
            {
                node = node.parent;
            }
            else
            {
                return node;
            }
        }
    }


    /// <summary>
    /// 插入碰撞器
    /// </summary>
    /// <param name="collider"></param>
    public void Insert(BaseCollider collider)
    {
        if (level >= MAX_LEVELS)
        {
            colliders.Add(collider);
            return;
        }
        // 没有子节点 直接插入本节点
        if (nodes == null)
        {
            colliders.Add(collider);
            if (colliders.Count > MAX_OBJECTS)
            {
                // 进行分裂,将完全属于子节点的碰撞器加入到子节点,并从当前节点移除
                Split();
                for (int i = colliders.Count - 1; i >= 0; i--)
                {
                    var index = GetIndex(colliders[i]);
                    if (index != -1)
                    {
                        nodes[index].Insert(colliders[i]);
                        colliders.RemoveAt(i);
                    }
                }
            }
        }
        else
        {
            var index = GetIndex(collider);
            if (index != -1)
            {
                nodes[index].Insert(collider);
            }
            else
            {
                colliders.Add(collider);
            }
        }
        // if (level >= MAX_LEVELS)
        // {
        //     colliders.Add(collider);
        //     return;
        // }
        // if (nodes != null)
        // {
        //     InsertChild(collider);
        // }
        // else
        // {
        //     colliders.Add(collider);
        //     if (colliders.Count > MAX_OBJECTS)
        //     {
        //         // 进行分裂,并将所有碰撞器加入到下面的节点,清空当前碰撞器列表
        //         Split();
        //         foreach (var item in colliders)
        //         {
        //             InsertChild(item);
        //         }
        //         colliders.Clear();
        //     }
        // }
    }

    /// <summary>
    /// 清空树
    /// </summary>
    public void Clear()
    {
        colliders.Clear();
        if (nodes != null)
        {
            foreach (var item in nodes)
            {
                item.Clear();
            }
        }
        nodes = null;
    }
}