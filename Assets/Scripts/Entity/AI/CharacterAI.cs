using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : ICharacterAI
{

    public CharacterAI(Entity Character):base(Character){
        ChangeAIState(new IdleAIState());
    }
    //是否可以攻击Heart
    public override bool CanAttackHeart(){
       return false;
    }
}
