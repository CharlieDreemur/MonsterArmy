using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//常数集，一些所有character共用的属性，不会更改


[System.Serializable]
public abstract class EntityData : IDataWithPrefab
{
    public DataBaseInfo baseInfo;

    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("角色模型")]
    [LabelWidth(70f)]
    [SerializeField]

    private UnityEngine.GameObject prefab;
    public override UnityEngine.GameObject Prefab
    {
        get
        {
            return prefab;
        }
        set
        {
            prefab = value;
        }
    }

    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("颜色")]
    [LabelWidth(70f)]
    public Color color = Color.white; 

    
    [FoldoutGroup("角色基本设置")]
    [LabelText("材质")]
    [LabelWidth(70f)]
    public Material material;


    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("大小")]
    [LabelWidth(70f)]
    public Vector3 scale = new Vector3(8f,8f,0f);


    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("攻击类型")]
    [LabelWidth(70f)]

    public Enum_AttackType attackType = Enum_AttackType.none;

    [FoldoutGroup("角色基本设置")] [LabelText("攻击动画类型")]
    public Enum_AttackAnimationType attackAnimationType;

    [ShowIf("attackType", Enum_AttackType.rangedAttack)]
    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("投掷物数据")]
    [LabelWidth(70f)]
    public ProjectileData projectileData; //投掷物

    [ShowIf("attackType", Enum_AttackType.rangedAttack)]
    [FoldoutGroup("角色基本设置")]
    [HideLabel]
    [LabelText("枪口位置")]
    [LabelWidth(70f)]
    public Vector3 relativePos; //枪口（投掷物实例的位置）


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
