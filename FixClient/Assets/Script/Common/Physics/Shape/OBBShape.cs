using System.Text;
using System.Collections.Generic;
using TrueSync;
namespace FixSystem
{

    public struct OBBShape : IShape
    {
        /// <summary>
        /// 顶点向量
        /// </summary>
        public List<TSVector2> vertexs { get; private set; }
        /// <summary>
        /// 投影轴
        /// </summary>
        public List<TSVector2> projections { get; private set; }

        /// <summary>
        /// 将凸多边形转换为OBB包围框
        /// </summary>
        /// <param name="vertexs">多边形的顶点向量</param>
        /// <param name="edgeVectors">多边形的边向量</param>
        public OBBShape(List<TSVector2> vertexs)
        {
            if (vertexs.Count < 3)
                throw new System.Exception("该多边形的顶点数量小于2,不能构成多边形");
            List<TSVector2> edgeVectors = new List<TSVector2>();
            for (int i = 0; i < vertexs.Count - 1; i++)
            {
                edgeVectors.Add(vertexs[i + 1] - vertexs[i]);
            }
            edgeVectors.Add(vertexs[0] - vertexs[vertexs.Count - 1]);
            this.vertexs = vertexs;
            this.projections = new List<TSVector2>();
            // 根据边向量求投影向量,即边向量的法向量(单位向量)
            // 根据垂直向量之间的关系:x1x2 + y1y2 = 0;
            // (x,y)的法向量为(-y,x)或者(y,-x);
            foreach (var item in edgeVectors)
            {
                this.projections.Add(new TSVector2(item.y, -item.x).normalized);
            }
        }




        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in vertexs)
            {
                builder.Append(item + "\n");
            }
            return builder.ToString();
        }


        public void Draw()
        {
            // if (vertexs.Count < 2)
            // {
            //     return;
            // }
            // for (int i = 0; i < vertexs.Count - 1; i++)
            // {
            //     Gizmos.DrawLine(vertexs[i], vertexs[i + 1]);
            // }
            // Gizmos.DrawLine(vertexs[vertexs.Count - 1], vertexs[0]);
        }

        public List<TSVector2> GetTrueVertexs()
        {
            return vertexs;
        }
    }
}