using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAIState : IAIState
{
   bool b_SetAttackPos = false; //是否设置了攻击目标
   public IdleAIState(){}
   //设置要攻击的目标
   public override void SetAttackPosition(Vector3 AttackPosition){
       b_SetAttackPos = true;
       //unfinished
   }

   //更新
   public override void Update(List<Entity> Targets){
        //没有目标时
        if(Targets == null || Targets.Count == 0)
        {
            //有设置攻击地点时，往攻击地点移动
            if(b_SetAttackPos){
                charAI.ChangeAIState(new MoveAIState());
            }
            return;
        }
        //找出最近的目标
        Entity theNearTarget = FindCloestTarget(Targets);
        
        //没有目标，会不动
        if(theNearTarget ==null){
            charAI.Idle();
            return;
        }

        //是否在距离内
        if(charAI.IsTargetInAttackRange(theNearTarget)){
            charAI.ChangeAIState(new AttackAIState(theNearTarget));
        }
        else{
            charAI.ChangeAIState(new ChaseAIState(theNearTarget));
        }
       
   }
       
   
}
