using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float Armor { get; set; }
    public float MaxArmor { get; set; }
    public float DamageReceivedModifier { get; set; } // Raw damage is multiplied by damage multiplier before damage is applied 
    public Vector3 KnockbackDirection { get; set; }
    //public BuffTarget BuffTarget { get; set; }

    public virtual void ApplyDamage(DamageInfo damageInfo)
    {
        TakeDamage(damageInfo.damage);
        OnApplyDamage(damageInfo);
        //Debug.Log("bruh");
        Vector3 knockbackDisplacement = KnockbackDirection * damageInfo.knockback;
        TakeKnockback(knockbackDisplacement);
        // if (BuffTarget is not null)
        // {
        //     BuffTarget.ApplyBuffs(damageInfo.BuffsToApply);
        // }
    }

    public virtual void TakeDamage(float damage)
    {
        damage *= DamageReceivedModifier; // Apply damage modifier before any calculation

        // if (BuffTarget is not null)
        // {
        //     damage *= BuffTarget.DefenseDamageModifier;
        // }
        if (Armor > 0.0f)
        {
            Armor -= damage;
            Armor = Mathf.Min(Armor, MaxArmor);
            if (Armor <= 0.0f)
            {
                Armor = 0.0f;
                OnArmorBreak();
            }
        }
        else
        {
            Health -= damage;
            Health = Mathf.Min(Health, MaxHealth);
            if (Health <= 0.0f)
            {
                Health = 0.0f;
                OnDeath();
            }
        }
        OnDamageOrHeal(damage);
    }

    public virtual void Heal(float amount)
    {
        Health = Mathf.Min(Health + amount, MaxHealth);
        OnDamageOrHeal(-amount);
    }

    public virtual void OnApplyDamage(DamageInfo damageInfo) {}

    //used for HUD updates
    public virtual void OnDamageOrHeal(float damage) {}

    //used for AI
    public virtual void OnArmorBreak() {}

    public virtual void TakeKnockback(Vector3 displacement)
    {
        return;
    }

    public void SetKnockbackDirection(Vector3 direction)
    {
        direction.y = 0.0f;
        KnockbackDirection = direction.normalized;
    }

    public virtual bool HasArmor() //For knockback and stunning
    {
        return Armor > 0.0f;
    }

    public abstract void OnDeath();
}