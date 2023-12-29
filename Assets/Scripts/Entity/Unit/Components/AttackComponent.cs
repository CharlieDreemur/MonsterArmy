using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MonsterArmy.Core.UnitSystem.Components
{
    public class AttackComponent : MonoBehaviour, IAttackComponent
    {

        public void TryAttack(Unit attacker, Unit target)
        {
            DamageInfo damageInfo = attacker.GetCharacterAttribute().GetAttackDamageInfo();
            StartCoroutine(DelayAttack(attacker.GetCharacterAttribute().attributeData.fixedData.AtkInterval * 0.6f, damageInfo, target));
        }

        IEnumerator DelayAttack(float seconds, DamageInfo damageInfo, Unit target)
        {
            yield return new WaitForSeconds(seconds);
            //Debug.Log("AttackWait"+Time.time);
            target.TakeDamage(damageInfo);
        }


    }
}