using UnityEngine;
public class PhysicsMono : MonoBehaviour
{
    /// <summary>
    /// 四叉树的范围
    /// </summary>
    public Vector2 size;
    public PhysicsWorld world;
    private void Start()
    {
        world = new PhysicsWorld(size);
        world.AddCollider(GameObject.FindObjectsOfType<BaseCollider>());
    }
    private void Update()
    {
        world.Update();
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(-size.x, -size.y), new Vector2(-size.x, size.y));
        Gizmos.DrawLine(new Vector2(-size.x, size.y), new Vector2(size.x, size.y));
        Gizmos.DrawLine(new Vector2(size.x, size.y), new Vector2(size.x, -size.y));
        Gizmos.DrawLine(new Vector2(size.x, -size.y), new Vector2(-size.x, -size.y));

        if (world != null && world.tree != null)
        {
            var leafNodes = world.tree.GetLeafNodes();
            foreach (var item in leafNodes)
            {
                Gizmos.DrawLine(item.bound.leftDown, item.bound.leftTop);
                Gizmos.DrawLine(item.bound.leftTop, item.bound.rightTop);
                Gizmos.DrawLine(item.bound.rightTop, item.bound.rightDown);
                Gizmos.DrawLine(item.bound.rightDown, item.bound.leftDown);
            }
        }
    }
}