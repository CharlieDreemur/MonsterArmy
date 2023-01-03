using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDebugger : MonoBehaviour
{
//一个负责监视ICharacterAttribute及其他变量的调试工具
    public EntityController manager = null;
    public ICharacter charController = null;
    public ICharacterAttribute charAttr = null;
    public string characterName;
    public Vector3 Transform_SpawnPos = Vector3.zero; //ICharacter出生的坐标
    public AttributeDataFixed fixedData;
    void Awake(){
        manager = UnityEngine.GameObject.FindWithTag("MainCamera").GetComponent<EntityController>();
    }

    void Start(){
        manager.PrintDic();
        charController = manager.GetCharacter(gameObject);
        
        charAttr = charController.GetCharacterAttribute();
    }
   
    // void Update(){
    //     characterName = charController.GetCharacterAttribute().CharacterName;
    //     attributeData = charController.GetCharacterAttribute().attributeData;
    // }
}
