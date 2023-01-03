using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[EnumToggleButtons]
public enum Enum_Character
{
    ICharacter,
    Character,
    Enemy
}

[EnumToggleButtons]
public enum Enum_CharacterDirection{
    right,
    left,
}

[EnumToggleButtons]
public enum Enum_DamageType{
    normal,
    ability, //所有的技能伤害
    crit,
    heal,
}


[EnumToggleButtons]
public enum Enum_AttackType
{
    none, //不攻击
    attack, //近战攻击（单体）
    rangedAttack, //远程攻击（单体）
}

[EnumToggleButtons]
public enum Enum_AttackAnimationType{
    sword, //挥砍
    bow, //拉弓
    magic, //魔法
}

[EnumToggleButtons]
public enum Enum_AnimationType{
    Idle,
    Run,
    Stun,
    Death,
    AttackSword,
    AttackBow,
    AttackMagic,
    SkillSword,
    SkillBow,
    SkillMagic,
}