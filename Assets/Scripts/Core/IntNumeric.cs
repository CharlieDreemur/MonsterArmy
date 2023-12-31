using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


    [System.Serializable]
    public struct IntNumeric
    {
        [SerializeField]
        [PropertyTooltip("Basic Value, positive integar only")]
        [HorizontalGroup("Attribute"), HideLabel]
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
        [PropertyTooltip("Extra Attribute"), HorizontalGroup("Attribute"), HideLabel]
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
        [PropertyTooltip("Percentage Attribute"), HorizontalGroup("Attribute"), HideLabel]
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

        [SerializeField, PropertyTooltip("Final Attribute"), HorizontalGroup("Attribute"), HideLabel]
        private int finalAttribute;
        /// <summary>
        /// Final Attribute = (Base Attribute + Add Attribute) * (1 + Pct Add Attribute)
        /// </summary>
        public int FinalAttribute
        {

            get
            {
                return (int)((BaseAttribute + AddAttribute) * (1 + PctAddAttribute) );
            }
        }

        public void Init()
        {
            BaseAttribute = 0;
            AddAttribute = 0;
            PctAddAttribute = 0f;
        }

        public void Init(IntNumeric value)
        {
            BaseAttribute = value.BaseAttribute;
            AddAttribute = value.AddAttribute;
            PctAddAttribute = value.PctAddAttribute;
        }

        public void Add(IntNumeric value)
        {
            BaseAttribute += value.BaseAttribute;
            AddAttribute += value.AddAttribute;
            PctAddAttribute += value.PctAddAttribute;
        }

        public void Minus(IntNumeric value)
        {
            BaseAttribute -= value.BaseAttribute;
            AddAttribute -= value.AddAttribute;
            PctAddAttribute -= value.PctAddAttribute;
        }


        public void Reset()
        {
            BaseAttribute = AddAttribute = 0;
            PctAddAttribute = 0.0f;
        }
    }

