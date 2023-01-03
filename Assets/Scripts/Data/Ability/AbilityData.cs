using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[EnumToggleButtons]
public enum Enum_CostType{
    none, //技能不消耗
    mp, //技能消耗法力值
    rage, //技能消耗怒气值
    hp, //技能消耗血量
}

[EnumToggleButtons]
public enum Enum_AbilityType{
    active,  ///主动技能
    passive, ///被动技能
}

//技能释放类型
[EnumToggleButtons] 
public enum Enum_AbilityReleaseType{
    instanrly,  ///瞬间释放
    channel, ///引导技能（在技能释放前需要引导时间）
    
}



[Serializable]   
///主动技能的基本单位
/// 由cost, cooldown
[CreateAssetMenu(fileName = "Ability", menuName = "TheMonsterArmy/AbilitySystem/Ability", order = 0)]
public class AbilityData : IData
{
    public DataBaseInfo baseInfo;
    
    ///TODO: Tag, Icon, Audio,
    /// 
    [FoldoutGroup("Setup")] [LabelText("AbilityType")]
    public Enum_AbilityType abilityType;

    
    [ShowIf("abilityType", Enum_AbilityType.active)] [FoldoutGroup("Setup")] //[LabelText("消耗类型")]
    public Enum_CostType costType; 

    [HideIf("costType", Enum_CostType.none)] [FoldoutGroup("Setup")] //[LabelText("消耗法力值")]
    public int costValue; ///消耗的值


    [ShowIf("abilityType", Enum_AbilityType.active)] [FoldoutGroup("Setup")] //[LabelText("冷却时间")]
    public float cooldown; ///冷却时间

    [FoldoutGroup("Setup")] //[LabelText("技能释放类型")]
    public Enum_AbilityReleaseType releaseType;
    [ShowIf("releaseType", Enum_AbilityReleaseType.channel)] [FoldoutGroup("Setup")] //[LabelText("引导时间")]
    public float channelTime; ///冷却时间
    
    [FoldoutGroup("Setup")] //[LabelText("角色释放动画")]
    public Enum_AnimationType animationType;

    public TargetChooserField targetChooser;

    //[LabelText("特效列表")] 
    public List<VFX> List_VFX = new List<VFX>(); //游戏特效预制体

    /*
    * isRange为ture时，TargetChooser只在range的范围里选择
    */

    //[LabelText("技能效果列表")] 
    public List<GameEffectData> List_GameEffectData = new List<GameEffectData>(); //技能效果表


    public void Copy(AbilityData data){
        baseInfo = data.baseInfo;
        abilityType = data.abilityType;
        costType = data.costType;
        costValue = data.costValue;
        cooldown = data.cooldown;
        releaseType = data.releaseType;
        channelTime = data.channelTime;
        animationType = data.animationType;
        List_VFX = data.List_VFX;
        List_GameEffectData = data.List_GameEffectData;
    }

}

