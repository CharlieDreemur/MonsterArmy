using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using YamlDotNet.Serialization;
namespace MonsterArmy.Core.UnitSystem
{
    [System.Serializable]
    [HideLabel]
    public class UnitAttribute
    {
        [Title("Basic Attribute")]
        [GUIColor(0f, 1f, 1f, 1f)]
        [LabelWidth(70f)]
        public string Name;
        [GUIColor(0f, 1f, 1f, 1f)]
        [LabelWidth(70f)]
        [MultilineAttribute]
        public string Description;

        [MinValue(0)]
        [GUIColor(0f, 1f, 0.6f, 1f)]
        [LabelWidth(70f)]
        [LabelText("HP")]
        [SerializeField]
        private int hp;


        [MinValue(0)]
        [GUIColor(0f, 0.6f, 1f, 1f)]
        [LabelWidth(70f)]
        [LabelText("MP")]
        [SerializeField]
        private int mp;


        public int HP
        {
            set
            {
                hp = Mathf.Clamp(value, 0, maxhp.FinalAttribute);
            }
            get
            {
                return hp;
            }
        }
        public int MP
        {
            set
            {
                mp = Mathf.Clamp(value, 0, maxmp.FinalAttribute);
            }
            get
            {
                return mp;
            }
        }
        [Title("Detailed Attribute")]
        [GUIColor(0f, 1f, 0f, 1f)]
        [InlineProperty]
        [LabelWidth(70f)]
        public IntNumeric maxhp;

        [GUIColor(0.2f, 0.5f, 1f, 1f)]
        [InlineProperty]
        [LabelWidth(70f)]
        public IntNumeric maxmp;


        [GUIColor(1f, 0f, 0f, 1f)]
        [InlineProperty]
        [LabelWidth(70f)]
        public IntNumeric atk;
        [GUIColor(0.9f, 0.6f, 0f, 1f)]
        [InlineProperty]
        [LabelWidth(70f)]
        public IntNumeric def;
        [GUIColor(0.7f, 0.3f, 0.9f, 1f)]
        [InlineProperty]
        [LabelWidth(70f)]
        public IntNumeric ap;

        [Title("Special Attribute")]
        
        [GUIColor(0.5f, 0.8f, 1f, 1f)]
        [InlineProperty]
        [SerializeField]
        [LabelWidth(70f)]
        private FloatNumeric atkspd;

        [GUIColor(0.3f, 0.9f, 0.9f, 1f)]
        [InlineProperty]
        [SerializeField]
        [LabelWidth(70f)]
        private FloatNumeric movespd;

        [GUIColor(0.6f, 0.4f, 0.8f, 1f)]
        [InlineProperty]
        [SerializeField]
        [LabelWidth(70f)]
        //[LabelText("攻击范围")]
        private FloatNumeric atkRange;

        [GUIColor(1f, 0.8f, 0f, 1f)]
        [InlineProperty]
        [SerializeField]
        [LabelWidth(70f)]
        [LabelText("Crit%")]
        private FloatNumeric critChance;

        [GUIColor(1f, 0.5f, 0f, 1f)]
        [InlineProperty]
        [SerializeField]
        [LabelWidth(70f)]
        [LabelText("CritDmg")]
        private FloatNumeric critDamage;

        [GUIColor(0.6f, 1f, 0.2f, 1f)]
        [InlineProperty]
        [SerializeField]
        [LabelWidth(70f)]
        [LabelText("Dodge%")]
        private FloatNumeric dodgeChance;

        [SerializeField]
        private Enum_AttackType attackType;
        [SerializeField]
        private Enum_AttackAnimationType attackAnimationType;

        public float DamageReduction
        {
            get
            {
                return (0.9f * def.FinalAttribute) / (20f + def.FinalAttribute);
            }
        }
        public float ATKSPD
        {
            get
            {
                return atkspd.FinalAttribute;
            }
        }

        [ShowInInspector]
        public float AtkInterval
        {
            get
            {
                if (ATKSPD > -1f)
                {
                    return 2 / (1 + ATKSPD);
                }
                else
                {
                    return 99999;
                }

            }
        }

        public float MoveSpeed
        {
            get
            {
                return movespd.FinalAttribute;
            }
        }

        public float AtkRange
        {
            get
            {
                return atkRange.FinalAttribute;
            }
        }

        public float CritChance
        {
            get
            {
                return critChance.FinalAttribute;
            }
        }

        public float CritDamage
        {
            get
            {
                return critDamage.FinalAttribute;
            }
        }

        public float DodgeChance
        {
            get
            {
                return dodgeChance.FinalAttribute;
            }
        }


        /// <summary>
        /// Init all attribute according to config, fill up hp and mp
        /// </summary>
        /// <param name="config"></param>
        public void Init(UnitAttributeConfig config)
        {
            Name = config.Name;
            Description = config.Description;
            maxhp.BaseAttribute = config.MAXHP;
            maxmp.BaseAttribute = config.MAXMP;
            atk.BaseAttribute = config.ATK;
            def.BaseAttribute = config.DEF;
            ap.BaseAttribute = config.AP;
            atkspd.BaseAttribute = config.ATKSPD;
            movespd.BaseAttribute = config.MOVSPD;
            atkRange.BaseAttribute = config.ATKRange;
            critChance.BaseAttribute = config.CritChance;
            critDamage.BaseAttribute = config.CritDamage;
            dodgeChance.BaseAttribute = config.DodgeChance;
            attackType = config.AttackType;
            attackAnimationType = config.AttackAnimationType;
            hp = config.MAXHP;
            mp = config.MAXHP;
        }


        [Button("Reset", ButtonSizes.Medium)]
        public void Reset()
        {
            Name = "";
            Description = "";
            hp=0;
            mp=0;
            maxhp.Reset();
            maxmp.Reset();
            atk.Reset();
            def.Reset();
            ap.Reset();
            atkspd.Reset();
            movespd.Reset();
            movespd.BaseAttribute = 2f;
            atkRange.Reset();
            atkRange.BaseAttribute = 5f;
            critChance.Reset();
            critDamage.Reset();
            critDamage.BaseAttribute = 1.5f;
            dodgeChance.Reset();
            attackType = Enum_AttackType.None;
            attackAnimationType = Enum_AttackAnimationType.Sword;
        }

        [Button("Clear", ButtonSizes.Medium)]
        public void Clear()
        {
            Name = "";
            Description = "";
            hp=0;
            mp=0;
            maxhp.Reset();
            maxmp.Reset();
            atk.Reset();
            def.Reset();
            ap.Reset();
            atkspd.Reset();
            movespd.Reset();
            atkRange.Reset();
            critChance.Reset();
            critDamage.Reset();
            dodgeChance.Reset();
            attackType = Enum_AttackType.None;
            attackAnimationType = Enum_AttackAnimationType.Sword;
        }


    }

}