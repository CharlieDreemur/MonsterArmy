using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[EnumToggleButtons]
public enum DamageType
{
    AttackDamage,
    AbilityPowerDamage,
    MixedDamage,
    TureDamage,

}

[System.Serializable]
public struct DamageInfo
{
    public DamageInfo(
        float damage,
        Unit instigator, 
        float knockback = 0.0f,
        //Buff
        DamageType damageType = DamageType.AttackDamage
    )
    {
        //Debug.Assert(damageIn >= 0.0f);

        this.damage = damage;
        this.knockback = knockback;

        this.attacker = instigator;
        this.damageType = damageType;
        /*
        if (buffsToApplyIn is null)
        {
            BuffsToApply = new List<BuffFactory>();
        }
        else
        {
            BuffsToApply = buffsToApplyIn;
        }*/
    }

    public float damage;
    public float knockback;
    //public List<BuffFactory> BuffsToApply;
    public DamageType damageType;
    public Unit attacker;

    
}
