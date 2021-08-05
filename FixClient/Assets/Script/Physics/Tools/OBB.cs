using System.Collections.Generic;
using UnityEngine;


/*
    OBB算法
*/
[System.Serializable]
public struct OBB
{
    /// <summary>
    /// 顶点向量
    /// </summary>
    public List<Vector2> vertexs { get; private set; }
    /// <summary>
    /// 投影轴
    /// </summary>
    public List<Vector2> projections { get; private set; }



    /// <summary>
    /// 将凸多边形转换为OBB包围框
    /// </summary>
    /// <param name="vertexs">多边形的顶点向量</param>
    /// <param name="edgeVectors">多边形的边向量</param>
    public OBB(List<Vector2> vertexs, List<Vector2> edgeVectors)
    {
        if (vertexs.Count < 3)
            throw new System.Exception("该多边形的顶点数量小于2,不能构成多边形");
        if (vertexs.Count != edgeVectors.Count)
            throw new System.Exception("该多边形的顶点和边向量的数量不一致");
        this.vertexs = vertexs;
        this.projections = new List<Vector2>();
        // 根据边向量求投影向量,即边向量的法向量(单位向量)
        // 根据垂直向量之间的关系:x1x2 + y1y2 = 0;
        // (x,y)的法向量为(-y,x)或者(y,-x);
        foreach (var item in edgeVectors)
        {
            this.projections.Add(new Vector2(item.y, -item.x).normalized);
        }
    }
}