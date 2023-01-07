using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System;

[EnumToggleButtons]
public enum Enum_EffectTime{
    instant, //立刻, buff,单次伤害和治疗请使用这个
    duration, //持续一段时间，只有多次伤害才使用
}

[EnumToggleButtons]
public enum Enum_EffectType{
    damage, //直接造成伤害或治疗
    buff, //给予某方面的属性
    projectile, //创建实体

}
//ps:对于同时具有以上三种效果的gameEffect，我们会创建多个gameEffect，并添加进ability的List_GameEffect里，按照一定顺序激活
[EnumToggleButtons]
public enum Enum_EffectHurtOrHeal{
    hurt,
    heal,
}
public enum Enum_BuffAttributeType{
    none,
    hp,
    atk,
    def,
    mp,
    attackSpeed,
    moveSpeed,
    shield, ///护盾值
    fire, ///焚烧效果
    poison, ///中毒效果
    stun, ///眩晕效果
    silent, ///沉默效果
    
}



[Serializable] 
/// <summary>
/// 技能系统基本组件——GameEffect是技能的controller，它负责控制，创建buff，给予buff数据和协调相应的特效
/// </summary>
/// 
[CreateAssetMenu(fileName = "GameEffect", menuName = "TheMonsterArmy/AbilitySystem/GameEffect", order = 0)]
public class GameEffectData : ScriptableObject
{
    
    public DataBaseInfo baseInfo;
    
    [FoldoutGroup("GameEffect基本设置")]
    public Enum_EffectTime effectTime; 
    [ShowIf("effectTime", Enum_EffectTime.duration)] [FoldoutGroup("GameEffect基本设置")]
    public float durationTime; ///持续时间
    
    [Tooltip("projectile仅对instant有效,不支持duration")] [FoldoutGroup("GameEffect基本设置")]
    public Enum_EffectType effectType;

    [FoldoutGroup("GameEffect基本设置")]
    public Enum_EffectHurtOrHeal hurtOrHeal;
    [FoldoutGroup("GameEffect基本设置")] [ShowIf("effectType", Enum_EffectType.projectile)] 
    public ProjectileData projectileData; ///投掷物
    
    [FoldoutGroup("GameEffect基本设置")] [ShowIf("effectType", Enum_EffectType.buff)] 
    public List<BuffData> buffData = new List<BuffData>();


    [FoldoutGroup("GameEffect Event设置")]
    public UnityEvent startEvent; ///开始效果时触发的事件
    [FoldoutGroup("GameEffect Event设置")]
    public UnityEvent chantEvent; ///吟唱结束时触发的事件
    [FoldoutGroup("GameEffect Event设置")]
    public UnityEvent endEvent; ///结束效果时触发的事件
    [FoldoutGroup("GameEffect数值设置")]
    
    //TODO: 将attributeDataGE做成列表，其中的index为level，等级不同的技能属性加成不同
    public AttributeDataGameEffect attributeDataGE;
   
}

