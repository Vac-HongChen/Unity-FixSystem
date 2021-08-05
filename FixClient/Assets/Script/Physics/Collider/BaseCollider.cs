using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class BaseCollider : MonoBehaviour
{
    /// <summary>
    /// 获取碰撞器的形状信息,用于做碰撞检测
    /// </summary>
    public abstract IShape GetShape();
    /// <summary>
    /// 获取碰撞器的矩形信息,即AABB包围框,用于添加到碰撞检测的四叉树中
    /// </summary>
    public abstract Rectangle GetRectangle();
    public PhysicsWorld world;

    // 在没有继承MonoBehaviour时,则需要实现以下参数
    /// <summary>
    /// 缩放
    /// </summary>
    public Vector2 LocalScale => transform.localScale;
    /// <summary>
    /// 世界坐标
    /// </summary>
    public Vector2 Position => transform.position;
    /// <summary>
    /// 逆时针旋转角度(即沿逆时针旋转多少度,沿顺时针则为负数)
    /// </summary>
    public float Angle => transform.rotation.eulerAngles.z;
    /// <summary>
    /// 当前挂载的SpriteRenderer
    /// </summary>
    public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();


    public List<BaseCollider> lastCollisionColliders = new List<BaseCollider>();
    public List<BaseCollider> curCollisionColliders = new List<BaseCollider>();
    public event Action<BaseCollider> OnColliderEnter;
    public event Action<BaseCollider> OnColliderStay;
    public event Action<BaseCollider> OnColliderExit;


    private void Awake()
    {
        // 可以在这里直接加入到碰撞世界中
    }
    private void OnDestroy()
    {
        // world.RemoveCollider(this);
    }


    /// <summary>
    /// 当前帧碰撞到物体
    /// 加入到当前帧队列
    /// 如果上一帧列表中存在,就触发持续碰撞
    /// 如果上一帧不存在,就触发第一次碰撞
    /// </summary>
    public void Collision(BaseCollider collider)
    {
        curCollisionColliders.Add(collider);
        if (lastCollisionColliders.Contains(collider))
        {
            OnColliderStay?.Invoke(collider);
            return;
        }
        OnColliderEnter?.Invoke(collider);
    }
    /// <summary>
    /// 当碰撞帧结束后触发,刷新当前的碰撞列表
    /// 遍历上一帧的碰撞列表,如果当前帧的碰撞列表不存在,则触发退出碰撞
    /// 清空上一帧的碰撞列表,将当前碰撞列表加入上一帧的碰撞列表
    /// 清空当前帧的碰撞列表
    /// </summary>
    public void RefreshColliderInfo()
    {
        foreach (var item in lastCollisionColliders)
        {
            if (!curCollisionColliders.Contains(item))
            {
                OnColliderExit?.Invoke(item);
            }
        }
        lastCollisionColliders.Clear();
        lastCollisionColliders.AddRange(curCollisionColliders);
        curCollisionColliders.Clear();
    }

    private void OnDrawGizmos()
    {
        // 矩形包围框描边
        // Gizmos.color = Color.red;
        // var rect = GetRectangle();
        // Gizmos.DrawLine(rect.leftDown, rect.leftTop);
        // Gizmos.DrawLine(rect.leftTop, rect.rightTop);
        // Gizmos.DrawLine(rect.rightTop, rect.rightDown);
        // Gizmos.DrawLine(rect.rightDown, rect.leftDown);


        Gizmos.color = Color.green;
        // 碰撞器描框
        GetShape().Draw();
    }

    private void OnDrawGizmosSelected()
    {

    }
}