using System.Diagnostics;
using System;

namespace FixSystem
{
    public class BattleEntity : Entity
    {
        public Action<int> OnReleaseSkill;
        public BattleEntity(World world) : base(world)
        {

        }
        public void ReleaseSkill(ReleaseData releaseData)
        {
            OnReleaseSkill?.Invoke(releaseData.skillId);
            // 创建技能物体
            SkillEntity entity = new SkillEntity(this.world, this, releaseData);
            entity.transform.position = transform.position;
        }
    }
}