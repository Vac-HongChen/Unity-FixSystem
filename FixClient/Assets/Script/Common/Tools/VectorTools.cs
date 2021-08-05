
using TrueSync;
namespace FixSystem
{
    public static class VectorTools
    {
        /// <summary>
        /// 角度转向量(已经归一化)
        /// </summary>
        public static TSVector2 AngleToVector(int angle)
        {
            var rad = angle * TSMath.Deg2Rad;
            var y = TSMath.Sin(rad);     // 求出斜边为1时的对边  y
            var x = TSMath.Cos(rad);     // 求出斜边为1时的临边  x
            var vector = new TSVector2(x, y);
            vector.Normalize();
            return vector;
        }

        /// <summary>
        /// 向量转角度
        /// </summary>
        public static int VectorToAngle(TSVector2 dir)
        {
            dir.Normalize();
            var angle = TSMath.Atan2(dir.y, dir.x) * TSMath.Rad2Deg;
            return (int)angle;
        }

    }
}
