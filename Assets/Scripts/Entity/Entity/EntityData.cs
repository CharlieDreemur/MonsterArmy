using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using YamlDotNet.Serialization;
using System.IO;

//常数集，一些所有character共用的属性，不会更改


[System.Serializable]
public abstract class EntityData : ScriptableObject, IPoolData
{
    public DataBaseInfo baseInfo;

    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("角色模型")]
    [LabelWidth(70f)]
    [SerializeField]

    private UnityEngine.GameObject prefab;
    public UnityEngine.GameObject Prefab{get => prefab; set => prefab = value;}

    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("颜色")]
    [LabelWidth(70f)]
    public Color color = Color.white; 


    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("大小")]
    [LabelWidth(70f)]
    public Vector3 scale = new Vector3(8f,8f,0f);


    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("攻击类型")]
    [LabelWidth(70f)]

    public Enum_AttackType attackType = Enum_AttackType.None;

    [FoldoutGroup("角色基本设置")] [LabelText("攻击动画类型")]
    public Enum_AttackAnimationType attackAnimationType;


    [FoldoutGroup("角色基本设置")]
    [ShowInInspector]
    [HideLabel]
    [LabelText("最大侦察范围")]
    [ReadOnly]
    [LabelWidth(80f)]
    public static float CONST_DETECT_RANGE = 999f; //默认最大侦察范围

    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("角色出生坐标")]
    [LabelWidth(80f)]
    public Vector3 Transform_SpawnPos = Vector3.zero; //ICharacter出生的坐标
    public AttributeDataField attributeData;
    

    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("技能配置")]
    [LabelWidth(80f)]
    public List<AbilityData> List_AbilityData; //关于技能的List
}
