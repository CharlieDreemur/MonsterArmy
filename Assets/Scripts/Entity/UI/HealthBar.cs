using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterArmy.Core.UnitSystem;
namespace MonsterArmy.UI
{

    public class HealthBar : MonoBehaviour
    {
        public Transform HealthFill; //HealthFill
        public Transform ShieldFill;
        public Transform MPFill;



        [SerializeField]
        private float HPscale = 1;

        [SerializeField]
        private float MPscale = 1;

        [SerializeField]
        private float Shieldscale = 0;

        [SerializeField]
        private bool IsInit;

        [SerializeField]
        private float Health_Width = 0f;

        [SerializeField]
        private Vector3 ShieldPos;

        [SerializeField]
        private float HealthFill_Pos_x;

        [SerializeField]
        private UnitAttributeController charAttr;


        /// <summary>
        /// Update只会在受到伤害时触发
        /// </summary>
        public void Update()
        {
            if (!IsInit || charAttr == null) return;
            int MaxValue = charAttr.Attribute.MAXHP + charAttr.Attribute.Shield;
            if (charAttr.Attribute.MAXHP != 0 || charAttr.Attribute.Shield != 0)
            {
                HPscale = (float)charAttr.Attribute.HP / MaxValue;
                Shieldscale = (float)charAttr.Attribute.Shield / MaxValue;
            }
            else
            {
                HPscale = 0;
                Shieldscale = 0;
            }

            if (charAttr.Attribute.MAXMP != 0)
            {
                MPscale = (float)charAttr.Attribute.MP / charAttr.Attribute.MAXMP;
            }
            else
            {
                MPscale = 0;
            }
            ShieldPos = new Vector3(HPscale * Health_Width + HealthFill_Pos_x, 0, 0);
            ShieldFill.localPosition = ShieldPos;
            HealthFill.localScale = new Vector3(HPscale, 1, 1);
            ShieldFill.localScale = new Vector3(Shieldscale, 1, 1);
            MPFill.localScale = new Vector3(MPscale, MPFill.localScale.y, MPFill.localScale.z);


        }

        [ContextMenu("TestPrint")]
        public void TestPrint(){
            charAttr.Attribute.TestPrint();
        }

        public void Init(Unit character)
        {
            Health_Width = HealthFill.GetComponent<Renderer>().bounds.size.x;
            HealthFill_Pos_x = HealthFill.localPosition.x;
            charAttr = character.AttributeController;
            IsInit = true;
        }




    }
}
