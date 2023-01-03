using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Enemy : Entity
{
    //创建时自动绑定
    // public Enemy(GameObject _obj, EnemyData _enemydata):base(_obj, _enemydata){
    //     Init(_obj, _enemydata);
    // }

      [FoldoutGroup("角色基本信息")]
      public new EnemyData data;
  
      public override void Init(ICharacterData data){

        this.data = data as EnemyData;
        //Debug.Log("Enemy.Init()");
        type_Character = Enum_Character.Enemy;
        SetCharacterAttribute(GetComponent<EnemyAttribute>());
        base.Init(data);

    }
  
}


