using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Friend : Entity

{
    // //创建时自动绑定
    // public Character(GameObject _obj, CharacterData _charData):base(_obj, _charData){
    //     Init(_obj, _charData);
    // }
    [FoldoutGroup("角色基本信息")]
    public new FriendData data;
    public override void Init(ICharacterData data){
        this.data = data as FriendData;
        type_Character = Enum_Character.Character;
        SetCharacterAttribute(GetComponent<CharacterAttribute>());
        base.Init(data);

    }
    

}
