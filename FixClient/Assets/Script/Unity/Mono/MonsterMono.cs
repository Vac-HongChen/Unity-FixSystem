using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixSystem;
using TrueSync;

public class MonsterMono : EntityMono<MonsterEntity>
{
    public override void Init()
    {
        entity.OnLogicUpdate += OnLogicUpdate;
    }
    public override void RenderUpdate()
    {
        // // transform.position = entity.transform.position.ToVector2();
        // // return;
        // var target = entity.operation.clickPosition.ToVector2();
        // // var target = entity.transform.position.ToVector2();
        // var dir = target - (Vector2)transform.position;
        // if (dir.sqrMagnitude > 0.01f)
        // {
        //     dir.Normalize();
        //     transform.Translate(dir * (float)entity.speed * Time.deltaTime, Space.World);
        // }
        // else
        // {
        //     transform.position = target;
        // }
    }


    private void OnLogicUpdate(FP deltaTime)
    {
        // print(entity.operation.skillId);
        // print(Vector3.Distance(transform.position,entity.transform.position.ToVector2()));
    }

    private void OnDestroy()
    {
        // print(transform.position);
    }
}