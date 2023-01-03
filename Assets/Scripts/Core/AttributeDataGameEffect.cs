using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[EnumToggleButtons]
/// <summary>
/// 加成类型
/// </summary>
public enum Enum_PctAttribute{
    none, ///没有加成
    atk, ///攻击力加成
    ap, ///法强加成
    hp, ///血量加成
}

[HideLabel]
[System.Serializable]
public struct AttributeDataGameEffect
{
    public int baseValue;

    public Enum_PctAttribute pctType;
    [DisableIf("pctType", Enum_PctAttribute.none)]

    [LabelText("属性加成%")]
    public int pctValue;

  
}
