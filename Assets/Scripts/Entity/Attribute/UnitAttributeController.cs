using System.Data.Common;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MonsterArmy.Core.UnitSystem.Interface;

namespace MonsterArmy.Core.UnitSystem
{
    [System.Serializable]
    //Character的数据，用于计算和表示角色obj的属性（变量）
    public class UnitAttributeController : MonoBehaviour, IUnitComponent
    {
        [SerializeField]
        [Sirenix.OdinInspector.ReadOnly]
        private UnitAttributeConfigLoader configLoader;
        [Title("Config")]
        [SerializeField]
        private int id;
        [SerializeField]
        private Enum_UnitType unitType;
        [SerializeField]
        private Vector3 scale = Vector3.one;
        [SerializeField]
        private Vector3 spawnPos;
        [SerializeField]
        private UnitAttribute attribute;
        public Vector3 Scale
        {
            get
            {
                return scale;
            }
        }
        public Vector3 SpawnPos
        {
            get
            {
                return spawnPos;
            }
        }
        public int ID
        {
            get
            {
                return id;
            }
        }
        public Enum_UnitType UnitType
        {
            get
            {
                return unitType;
            }
        }
        public UnitAttribute Attribute
        {
            get
            {
                if (attribute == null) LoadConfig();
                return attribute;
            }
        }
        public void Init(IUnitComponentInitData initData)
        {
            LoadConfig();
        }
        [ContextMenu("Print")]
        public void PrintAttribute()
        {
            attribute.TestPrint();
        }
        [ContextMenu("Load Config")]
        private void LoadConfig()
        {
            configLoader = FindObjectOfType<UnitAttributeConfigLoader>();
            switch (unitType)
            {
                case Enum_UnitType.Ally:
                    attribute.Init(configLoader.allyConfig[id]);
                    break;
                case Enum_UnitType.Enemy:
                    attribute.Init(configLoader.enemyConfig[id]);
                    break;
            }
        }


        #region GetDamage方法组


        /*
        @funciton:给予两个人物，返回一方对另一方的一次攻击造成的伤害，考虑闪避，暴击，防御等因素
        */
        public static int CalculateDamage(Unit attacker, Unit target, out bool isDodge, out bool isCrit)
        {

            return CalculateDamage(attacker.Attribute, target.Attribute, out isDodge, out isCrit);
        }

        /*
        @function: 给予两个角色属性，返回一方对另一方的一次攻击造成的伤害，考虑闪避，暴击，防御等因素
        */
        public static int CalculateDamage(UnitAttribute attribute, UnitAttribute target_Attr, out bool isDodge, out bool isCrit)
        {
            isDodge = false;
            isCrit = false;

            int atkDamage = attribute.ATK;
            if (target_Attr.IsDodge())
            {

                isDodge = true;
                return 0;
            }

            if (attribute.IsCrit())
            {
                Debug.Log(attribute.Name + "暴击了");
                atkDamage = (int)(atkDamage * attribute.CritDamage);
                isCrit = true;
            }

            atkDamage = (int)(atkDamage * (1 - target_Attr.DamageReduction));
            if (atkDamage < 0)
            {
                atkDamage = 0;
            }
            return atkDamage;
        }

        #endregion

        public void TakeDamage(DamageInfo damageInfo, out int damage, out bool isDodge, out bool isCrit)
        {
            damage = CalculateDamage(damageInfo.attacker.Attribute, this.attribute, out isDodge, out isCrit);
            if (isDodge)
            {
                return;
            }
            UnderHurt(damage);
        }
        /// <summary>
        /// First Calculate Shield, then health
        /// </summary>
        /// <param name="damage"></param>
        public void UnderHurt(int damage)
        {
            if (attribute.Shield > 0)
            {
                if (attribute.Shield >= damage)
                {
                    attribute.Shield -= damage;
                }
                else
                {
                    damage -= attribute.Shield;
                    attribute.Shield = 0;
                    attribute.HP -= damage;

                }
            }
            else
            {
                attribute.HP -= damage;
            }
        }

        /*
        @function 使该角色增加_healAmount的血量
        */

        public void UnderHeal(int healAmount)
        {
            attribute.HP += healAmount;
        }
    }

}