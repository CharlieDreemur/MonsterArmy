using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


    /// <summary>
    /// 一层属性 = 基本属性,
    /// 二层属性 = 一层属性+装备属性+buff属性,
    /// 三层属性 = 二层属性*装备加成 * （1+装备百分比加成+buff百分比加成）
    /// 最终属性为三层属性
    /// </summary>

    [System.Serializable]
    public struct IntNumeric
    {
        [OnInspectorInit("Update")]
        [SerializeField]
        [PropertyTooltip("角色的基本属性，只能为正整数")]
        [OnValueChanged("Update"), HorizontalGroup("Attribute"), HideLabel]
        [MinValue(0)]
        private int baseAttribute;
        public int BaseAttribute
        {
            set
            {
                if (value > 0)
                {
                    baseAttribute = value;
                }
                else
                {
                    baseAttribute = 0;
                }
            }
            get
            {
                return baseAttribute;
            }
        }

        [SerializeField]
        [OnValueChanged("Update")]
        [PropertyTooltip("角色的附加属性"), HorizontalGroup("Attribute"), HideLabel]
        private int addAttribute;


        public int AddAttribute
        {
            set
            {
                addAttribute = value;
            }
            get
            {
                return addAttribute;
            }
        }

        [SerializeField]
        [OnValueChanged("Update")]
        [PropertyTooltip("角色的百分比加成属性"), HorizontalGroup("Attribute"), HideLabel]
        private float pctAddAttribute;

        public float PctAddAttribute
        {
            set
            {
                pctAddAttribute = value;
            }
            get
            {
                return pctAddAttribute;
            }
        }

        [SerializeField, PropertyTooltip("计算所有加成的最后的属性(正整数）"), HorizontalGroup("Attribute"), HideLabel]
        private int finalAttribute;

        public int FinalAttribute
        {
            set
            {
                if (value > 0)
                {
                    finalAttribute = value;
                }
                else
                {
                    finalAttribute = 0;
                }
            }

            get
            {
                return finalAttribute;
            }
        }

        public void Init()
        {
            BaseAttribute = AddAttribute;
            PctAddAttribute = 0f;
            Update();
        }

        public int SetBase(int value)
        {
            BaseAttribute = value;
            Update();
            return BaseAttribute;
        }


        /// <summary>
        /// 直接增加属性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(int value)
        {
            AddAttribute += value;
            Update();
            return AddAttribute;
        }

        /// <summary>
        /// 百分比增加属性，比如增加30%的攻击力，请输入30 而不是0.3
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float PctAdd(int value)
        {
            PctAddAttribute += value;
            Update();
            return PctAddAttribute;
        }

        public void Update()
        {
            FinalAttribute = (int)((BaseAttribute + AddAttribute) * (1 + PctAddAttribute) );
        }

        public void Init(IntNumeric value)
        {
            BaseAttribute = value.BaseAttribute;
            AddAttribute = value.AddAttribute;
            PctAddAttribute = value.PctAddAttribute;
            Update();
        }

        public void Add(IntNumeric value)
        {
            BaseAttribute += value.BaseAttribute;
            AddAttribute += value.AddAttribute;
            PctAddAttribute += value.PctAddAttribute;
            Update();
        }

        public void Minus(IntNumeric value)
        {
            BaseAttribute -= value.BaseAttribute;
            AddAttribute -= value.AddAttribute;
            PctAddAttribute -= value.PctAddAttribute;
            Update();
        }


        public void Reset()
        {
            BaseAttribute = AddAttribute = FinalAttribute = 0;
            PctAddAttribute = 0;
        }
    }

