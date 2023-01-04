using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Ally : Entity

{
    // //创建时自动绑定
    // public Character(GameObject _obj, CharacterData _charData):base(_obj, _charData){
    //     Init(_obj, _charData);
    // }
    [FoldoutGroup("角色基本信息")]
    public new AllyData data;
    public override void Init(EntityData data){
        this.data = data as AllyData;
        type_Character = Enum_Character.Character;
        SetCharacterAttribute(GetComponent<CharacterAttribute>());
        base.Init(data);

    }
    

}
