using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[EnumToggleButtons]
public enum UnitType
{
    None,
    Ally,
    Enemy
}

[EnumToggleButtons]
public enum Enum_FaceDirection{
    right,
    left,
}

[EnumToggleButtons]
public enum Enum_DamageType
{
    PhysicalDamage,
    MagicDamage,
    MixedDamage,
    TureDamage,
    Heal,

}


[EnumToggleButtons]
public enum Enum_AttackType
{
    None, //不攻击
    Attack, //近战攻击（单体）
    RangedAttack, //远程攻击（单体）
}

[EnumToggleButtons]
public enum Enum_AttackAnimationType{
    Sword, //挥砍
    Bow, //拉弓
    Magic, //魔法
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