using UnityEngine;


public abstract class EntityMono<Entity> : MonoBehaviour where Entity : FixSystem.Entity
{
    public Entity entity;
    public abstract void RenderUpdate();
    public abstract void Init();
    public Vector2 LogicPosition;

    private void Start()
    {
        transform.position = entity.transform.position.ToVector2();
        Init();
    }
    protected virtual void Update()
    {
        RenderUpdate();
        LogicPosition = entity.transform.position.ToVector2();


        // print("渲染位置:" + transform.position);
        // print("逻辑位置:" + LogicPosition);
        // transform.position = Vector3.Lerp(transform.position, LogicPosition, Time.deltaTime * 3);

        var dir = LogicPosition - (Vector2)transform.position;
        transform.position = (Vector2)transform.position + dir * Time.deltaTime * 20;
    }


    private void OnDrawGizmos()
    {
        // 矩形包围框描边
        if (entity.collider == null)
        {
            return;
        }
        Gizmos.color = Color.blue;
        var shapge = entity.collider.GetShape();
        var vertexs = shapge.GetTrueVertexs();
        if (vertexs.Count > 2)
        {
            for (int i = 0; i < vertexs.Count - 1; i++)
            {
                Gizmos.DrawLine(vertexs[i].ToVector2(), vertexs[i + 1].ToVector2());
            }
            Gizmos.DrawLine(vertexs[vertexs.Count - 1].ToVector2(), vertexs[0].ToVector2());
        }
        else
        {
            print("该形状的顶点小于3个,不能构成碰撞器:" + shapge);
        }
    }
}