using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


    [System.Serializable]
    public struct IntNumeric
    {
        [OnInspectorInit("Update")]
        [SerializeField]
        [PropertyTooltip("Basic Value, positive integar only")]
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
        [OnValueChanged("Update")]
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
        /// Add AddAttribute
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
        /// 0.1 = 10%
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

