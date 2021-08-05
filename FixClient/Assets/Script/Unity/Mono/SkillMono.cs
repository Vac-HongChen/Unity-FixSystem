using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixSystem;
using TrueSync;

public class SkillMono : EntityMono<SkillEntity>
{
    public override void Init()
    {
        entity.OnLogicUpdate += OnLogicUpdate;
    }

    public override void RenderUpdate()
    {
        
    }

    private void OnLogicUpdate(FP deltaTime)
    {
        // print(Vector3.Distance(transform.position,entity.transform.position.ToVector2()));
    }

    private void OnDestroy()
    {
        // print(transform.position);
        // print(entity.transform.position);
    }
}