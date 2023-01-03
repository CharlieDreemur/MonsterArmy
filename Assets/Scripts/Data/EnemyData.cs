using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[EnumToggleButtons]
public enum Enum_Enemy{
    none, //默认
    pawn, //小兵
    boss, //Boss
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "TheMonsterArmy/ScriptableObject/EnemyData", order = 1)]
public class EnemyData : ICharacterData
{
    [FoldoutGroup("敌方类型")]
    public Enum_Enemy type_enemy = Enum_Enemy.pawn;
}
