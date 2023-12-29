using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace MonsterArmy.Core.UnitSystem
{
    public class UnitAttributeContainer : MonoBehaviour
    {
        [Title("Config")]
        private UnitAttributeConfigLoader configLoader;
        [SerializeField]
        private int id;
        [SerializeField]
        public UnitType unitType;
        [SerializeField]
        public Vector3 scale = Vector3.one;
        [SerializeField]
        public Vector3 spawnPos;
        [SerializeField]
        private UnitAttribute attribute;
        public UnitAttribute Attribute
        {
            get
            {
                if (attribute == null) LoadConfig();
                return attribute;
            }
        }
        private void Awake()
        {
            LoadConfig();
        }
        [ContextMenu("Load Config")]
        private void LoadConfig()
        {
            configLoader = FindObjectOfType<UnitAttributeConfigLoader>();
            switch (unitType)
            {
                case UnitType.Ally:
                    attribute.Init(configLoader.allyConfig[id]);
                    break;
                case UnitType.Enemy:
                    attribute.Init(configLoader.enemyConfig[id]);
                    break;
            }
        }
    }
}