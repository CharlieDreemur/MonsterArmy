using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MonsterArmy.Core.UnitSystem.Components
{
    public class RangedAttackComponent : MonoBehaviour, IAttackComponent
    {
        public ProjectileData projectileData;
        public Vector3 shootPosOffset;
        public void TryAttack(Unit attacker, Unit target)
        {
            Vector3 projectileSpawnPos = transform.position;
            DamageInfo damageInfo = attacker.GetCharacterAttribute().GetAttackDamageInfo();
            switch(attacker.characterDirection){
                case Enum_FaceDirection.left:
                    projectileSpawnPos += new Vector3(-shootPosOffset.x, shootPosOffset.y, shootPosOffset.z);
                    break;
                case Enum_FaceDirection.right:
                    projectileSpawnPos += shootPosOffset;
                    break;
                default:
                    break;
            }

            ProjectileArgs projectileArgs = new ProjectileArgs(projectileData, projectileSpawnPos, damageInfo, target);
            Action action = () => EventManager.TriggerEvent("InstantiateProjectile", JsonUtility.ToJson(projectileArgs));
            StartCoroutine(DelayInvokeAction(action, attacker.GetCharacterAttribute().attributeData.fixedData.AtkInterval * 0.6f));
        }
        IEnumerator DelayInvokeAction(Action action, float delaySeconds)
        {
            if (delaySeconds > 0)
                yield return new WaitForSeconds(delaySeconds);
            else
                yield return null;
            action?.Invoke();
        }
    }
}