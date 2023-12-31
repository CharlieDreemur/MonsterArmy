using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "AbilityIndicatorData", menuName = "TheMonsterArmy/ScriptableObject/UISystem/AbilityIndicatorData", order = 0)]
public class AbilityIndicatorData : ScriptableObject
{
    [FoldoutGroup("技能指示器设置")] [HideLabel] [LabelText("模型")] [SerializeField]
    public UnityEngine.GameObject prefab;
}
