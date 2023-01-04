using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[EnumToggleButtons] [HideLabel] [LabelText("品质")] [LabelWidth(40f)]
public enum Enum_Grade{
    none,
    D,
    C,
    B,
    A,
    S,
}


[EnumToggleButtons] [HideLabel] [LabelText("职业")] [LabelWidth(40f)]
public enum Enum_Profession{
    none, //默认，没有职业
    warrior, //战士
    mage, //法师
    archer, //射手
    assassin, //刺客
}

[EnumToggleButtons] [HideLabel] [LabelText("星级")] [LabelWidth(40f)]
public enum Enum_Star{
    none, //默认，没有星级或0星级
    one, 
    two,
    three,
    four,
    five,
}
[CreateAssetMenu(fileName = "AllyData", menuName = "TheMonsterArmy/EntitySystem/AllyData", order = 0)]
//每个character的初始属性
//[read only]
public class AllyData : EntityData {
    [FoldoutGroup("英雄类型")]
    public Enum_Grade grade = Enum_Grade.none; //品质
    [FoldoutGroup("英雄类型")]
    public Enum_Profession profession = Enum_Profession.none; //职业
    [FoldoutGroup("英雄类型")]
    public Enum_Star star = Enum_Star.none; //默认0级

   

}

