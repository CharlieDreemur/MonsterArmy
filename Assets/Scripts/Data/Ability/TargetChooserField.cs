using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[EnumToggleButtons]
public enum Enum_EffectTargetType{
    enemy, //敌方

    friend, //友方
}

[EnumToggleButtons]
public enum Enum_EffectTargetChooser{
    current, //当前攻击目标/离当前目标最近的n个（包括当前目标，从离当前目标最近到最远排序）
    cloest, ///最近目标/最近的n个（从近到远排序）
    farwest, ///最远目标/最远的n个（从远到近排序）
    leastHP, //血量最少/血量最少n个
    mostHP, //血量最多/血量最多n个
    ranged, //在一定范围内的所有
}

[EnumToggleButtons]
public enum Enum_EffectTargetNumber{
    single, //单个目标
    multi, //多个目标
}

[System.Serializable] [HideLabel] 
public struct TargetChooserField{
    
   [OnValueChanged("OnTargetChooserChanged")]  //[LabelText("选择方式")] 
    public Enum_EffectTargetChooser effectTargetChooser;

    [OnInspectorInit("OnTargetNumberChanged")] [OnInspectorInit("OnTargetChooserChanged")]
    [OnValueChanged("OnTargetNumberChanged")] //[LabelText("单体/群体")]
    public Enum_EffectTargetNumber effectTargetNumber;
    //[LabelText("敌方/友方")]
    public Enum_EffectTargetType effectTargetType;
    [ShowIf("effectTargetNumber", Enum_EffectTargetNumber.multi)] [MinValue(0)] [MaxValue(99)] [HorizontalGroup("targetNumber")] [DisableIf("isTargetAll", true)] //[LabelText("选择目标个数")] 
    public int targetNumber; 

    [HideLabel] [ShowIf("effectTargetNumber", Enum_EffectTargetNumber.multi)] [HorizontalGroup("targetNumber")] 
    public bool isTargetAll; //选择全体
    [MinValue(0f)] [MaxValue(99f)] [HorizontalGroup("range")] [DisableIf("isRangeInfinity", true)] //[LabelText("范围半径")]
    public float range; //以ICharacter所在坐标为圆心的圆圈半径
    [HideLabel] [HorizontalGroup("range")] 
    public bool isRangeInfinity; //是否使用无限范围,为true的时候不会考虑范围限制

    [Button("Reset", ButtonSizes.Medium)]
    public void Reset(){
        effectTargetNumber = Enum_EffectTargetNumber.single;
        effectTargetChooser = Enum_EffectTargetChooser.current;
        effectTargetType = Enum_EffectTargetType.enemy;
        OnTargetNumberChanged();
    }

    public void OnTargetNumberChanged(){
        if(effectTargetNumber == Enum_EffectTargetNumber.single){
            targetNumber = 1;
            isTargetAll = false;
        }
    }

    public void OnTargetChooserChanged(){
        switch(effectTargetChooser){
            case Enum_EffectTargetChooser.ranged:
            isTargetAll = true;
            effectTargetNumber = Enum_EffectTargetNumber.multi;
            break;
        }
    }

}