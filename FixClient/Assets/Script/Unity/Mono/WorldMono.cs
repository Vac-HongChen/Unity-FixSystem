using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixSystem;
using TrueSync;
public class WorldMono : MonoBehaviour
{
    public Dictionary<Entity, GameObject> entityDict = new Dictionary<Entity, GameObject>();
    public PlayerMono playerAsset;
    public SkillMono skillAsset;
    public MonsterMono monsterAsset;

    public void BindLogic(LogicCore logicCore)
    {
        logicCore.OnAddEntity += OnAddEntity;
        logicCore.OnRemoveEntity += OnRemoveEntity;
    }


    public void OnAddEntity(Entity entity)
    {
        if (entityDict.ContainsKey(entity))
        {
            BattleDebug.LogError("场景添加了相同的单位");
            return;
        }
        if (entity is PlayerEntity)
        {
            var go = Instantiate(playerAsset.gameObject);
            entityDict.Add(entity, go);
            var mono = go.GetComponent<PlayerMono>();
            mono.entity = entity as PlayerEntity;
        }
        else if (entity is SkillEntity)
        {
            var go = Instantiate(skillAsset.gameObject);
            entityDict.Add(entity, go);
            var mono = go.GetComponent<SkillMono>();
            mono.entity = entity as SkillEntity;
        }
        else if (entity is MonsterEntity)
        {
            var go = Instantiate(monsterAsset.gameObject);
            entityDict.Add(entity, go);
            var mono = go.GetComponent<MonsterMono>();
            mono.entity = entity as MonsterEntity;
        }
        else
        {
            BattleDebug.LogError("没有该类型的Mono函数" + entity.GetType());
        }
    }

    public void OnRemoveEntity(Entity entity)
    {
        if (!entityDict.ContainsKey(entity))
        {
            BattleDebug.LogError("场景删除了不存在的单位");
            return;
        }
        Destroy(entityDict[entity]);
    }
}