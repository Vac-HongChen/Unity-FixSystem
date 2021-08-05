using TrueSync;

namespace FixSystem
{
    /// <summary>
    /// 技能释放参数
    /// </summary>
    public class ReleaseData
    {
        public int skillId;


        // 飞行类技能参数
        /// <summary>
        /// 释放时鼠标的位置,根据此位置计算飞行方向
        /// </summary>
        public TSVector2 mouseDir;
        /// <summary>
        /// 释放角度
        /// </summary>
        public int angle;
    }
}
