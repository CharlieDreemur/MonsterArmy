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
        ICharacter instigator = null, 
        float knockback = 0.0f,
        //List<BuffFactory> buffsToApplyIn = null,
        DamageType damageType = DamageType.AttackDamage
    )
    {
        //Debug.Assert(damageIn >= 0.0f);

        this.damage = damage;
        this.knockback = knockback;

        this.instigator = instigator;
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
    private ICharacter instigator;

    /*public Entity Instigator
    {
        get
        {
            if (instigator is null)
            {
                //Debug.LogWarning("Warning: Instigator is null!");
            }
            return instigator;
        }
        set => instigator = value;
    }
    */
}
