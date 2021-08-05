
using TrueSync;
using System.Collections.Generic;
namespace FixSystem
{
    public struct CircularShape : IShape
    {
        public FP radius { get; private set; }
        public TSVector2 center { get; private set; }

        public CircularShape(FP radius, TSVector2 center)
        {
            this.radius = radius;
            this.center = center;
        }

        public void Draw()
        {
            // var m_Theta = 0.0001f;
            // TSVector2 beginPoint = Vector3.zero;
            // TSVector2 firstPoint = Vector3.zero;
            // for (FP theta = 0; theta < 2 * TSMath.PI; theta += m_Theta)
            // {
            //     FP x = radius * TSMath.Cos(theta);
            //     FP y = radius * TSMath.Sin(theta);
            //     TSVector2 endPoint = new TSVector2(x, y);
            //     if (theta == 0)
            //     {
            //         firstPoint = endPoint;
            //     }
            //     else
            //     {
            //         Gizmos.DrawLine(beginPoint + center, endPoint + center);
            //     }
            //     beginPoint = endPoint;
            // }
            // Gizmos.DrawLine(firstPoint + center, beginPoint + center);
        }

        public List<TSVector2> GetTrueVertexs()
        {
            List<TSVector2> vertexs = new List<TSVector2>();
            var m_Theta = 0.1f;
            TSVector2 beginPoint = TSVector2.zero;
            TSVector2 firstPoint = TSVector2.zero;
            for (FP theta = 0; theta < 2 * TSMath.Pi; theta += m_Theta)
            {
                FP x = radius * TSMath.Cos(theta);
                FP y = radius * TSMath.Sin(theta);
                vertexs.Add(new TSVector2(x, y) + center);
            }
            return vertexs;
        }
    }
}