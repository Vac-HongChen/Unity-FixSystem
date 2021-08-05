using System.Collections.Generic;
using TrueSync;
namespace FixSystem
{
    public interface IShape
    {
        void Draw();
        List<TSVector2> GetTrueVertexs();
    }
}
