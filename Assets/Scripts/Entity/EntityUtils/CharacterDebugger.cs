using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDebugger : MonoBehaviour
{
//一个负责监视ICharacterAttribute及其他变量的调试工具
    public EntityManager manager = null;
    public Unit charController = null;
    public EntityAttribute charAttr = null;
    public string characterName;
    public Vector3 Transform_SpawnPos = Vector3.zero; //ICharacter出生的坐标
    public AttributeDataFixed fixedData;
    void Awake(){
        manager = UnityEngine.GameObject.FindWithTag("MainCamera").GetComponent<EntityManager>();
    }

    void Start(){
        charController = manager.GetEntity(gameObject);
        
        charAttr = charController.GetCharacterAttribute();
    }
   
    // void Update(){
    //     characterName = charController.GetCharacterAttribute().CharacterName;
    //     attributeData = charController.GetCharacterAttribute().attributeData;
    // }
}
