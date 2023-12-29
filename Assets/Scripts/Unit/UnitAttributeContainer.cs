using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace MonsterArmy.Core.Unit
{
    public class UnitAttributeContainer : MonoBehaviour
    {
        [Title("Config")]
        public int id;
        public UnitAttributeConfigLoader configLoader;
        public UnitType unitType;
        [SerializeField]
        private UnitAttribute attribute;
        public UnitAttribute Attribute{
            get{
                if(attribute == null) LoadConfig();
                return attribute;
            }
        }
        private void Awake() {
            LoadConfig();
        }
        [ContextMenu("Load Config")]
        private void LoadConfig(){
            switch(unitType){
                case UnitType.Ally:
                    attribute = new UnitAttribute(configLoader.allyConfig[id]);
                    break;
                case UnitType.Enemy:
                    attribute = new UnitAttribute(configLoader.enemyConfig[id]);
                    break;
            }
        }
    }
}