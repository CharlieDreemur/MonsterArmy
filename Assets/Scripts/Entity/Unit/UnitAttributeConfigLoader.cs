using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;

namespace MonsterArmy.Core.UnitSystem
{
    public class UnitAttributeConfigLoader : MonoBehaviour
    {
        public TextAsset enemyYamlConfig; // Drag and drop your YAML file here in the Inspector
        public TextAsset allyYamlConfig;
        [SerializeField]
        public List<UnitAttributeConfig> enemyConfig;
        [SerializeField]
        public List<UnitAttributeConfig> allyConfig;
        private void Awake() {
            LoadYamlConfig();
        }

        [ContextMenu("Load Yaml Config")]
        private void LoadYamlConfig()
        {                
            var deserializer = new DeserializerBuilder().Build();
            if (enemyYamlConfig != null)
            {

                enemyConfig = deserializer.Deserialize<List<UnitAttributeConfig>>(enemyYamlConfig.text);
            }
            else{
                Debug.LogError("Enemy YAML file is not assigned!");
            }

            if(allyYamlConfig != null){
                allyConfig = deserializer.Deserialize<List<UnitAttributeConfig>>(allyYamlConfig.text);
            }
            else{
                Debug.LogError("Ally YAML file is not assigned!");
            }
        }
    }
}