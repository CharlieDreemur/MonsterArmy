using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public struct DamageInfo
{
    public DamageInfo(
        float damage,
        Unit instigator, 
        bool isCrit=false,
        float knockback = 0.0f,
        //Buff
        Enum_DamageType damageType = Enum_DamageType.PhysicalDamage
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
    public Enum_DamageType damageType;
    public Unit attacker;

    
}
