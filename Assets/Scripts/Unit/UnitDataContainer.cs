using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;

namespace MonsterArmy.Core.Unit
{


    public class UnitDataContainer : MonoBehaviour
    {
        public TextAsset enemyYamlConfig; // Drag and drop your YAML file here in the Inspector
        public TextAsset allyYamlConfig;
        [SerializeField]
        private List<UnitAttributeConfig> enemyConfig;
        [SerializeField]
        private List<UnitAttributeConfig> allyConfig;

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