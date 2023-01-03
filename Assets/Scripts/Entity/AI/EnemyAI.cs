using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : ICharacterAI
{
    public EnemyAI(ICharacter Character):base(Character){
        ChangeAIState(new IdleAIState());
    }
    //是否可以攻击Heart
    public override bool CanAttackHeart(){
       return true;
    }
}
