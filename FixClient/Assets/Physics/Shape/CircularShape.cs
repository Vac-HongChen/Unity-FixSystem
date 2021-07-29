using UnityEngine;
public struct CircularShape : IShape
{
    public float radius { get; private set; }
    public Vector2 center { get; private set; }

    public CircularShape(float radius, Vector2 center)
    {
        this.radius = radius;
        this.center = center;
    }




    public void Draw()
    {
        var m_Theta = 0.0001f;
        Vector2 beginPoint = Vector3.zero;
        Vector2 firstPoint = Vector3.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector2 endPoint = new Vector2(x, y);
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Gizmos.DrawLine(beginPoint + center, endPoint + center);
            }
            beginPoint = endPoint;
        }
        Gizmos.DrawLine(firstPoint + center, beginPoint + center);
    }
}