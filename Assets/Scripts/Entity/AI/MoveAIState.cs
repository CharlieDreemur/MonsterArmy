using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//当转为IdleAIState时，若已经设置了攻击目标，则向攻击目标移动
public class MoveAIState : IAIState
{

   private const float MOVE_CHECK_DIST = 1.5f;//
   bool b_OnMove = false;

   Transform attackTransform = null;
   Vector3 attackPos = Vector3.zero;
   public MoveAIState(){}

   //设置要攻击到达目标
   public override void SetAttackPosition(Vector3 _attackPosition){
       attackPos = _attackPosition;
   }
    public override void SetAttackTransform(Transform _attackTransform){
       attackTransform = _attackTransform;
   }

   //更新
   public override void Update(List<Unit> Targets){
       //有目标时，改为待机状态
       if(Targets != null && Targets.Count > 0){
           charAI.ChangeAIState(new IdleAIState());
           return;
       }
  
    
       //已经目标移动
       if(b_OnMove){
           //是否到达
           //float dist = Vector3.Distance(attackPos, charAI.GetPosition());
           float dist = Vector3.Distance(attackTransform.position, charAI.GetPosition());
           if(dist < MOVE_CHECK_DIST){
               charAI.ChangeAIState(new IdleAIState());
               if(charAI.IsKilled() == false){
                   //占领领地(unfinished)
               }
               charAI.Killed();
           }
           return;
          
       }
        //往目标移动
           b_OnMove = true;
           //charAI.MoveTo(attackPos);
           charAI.MoveTo(attackTransform);
   }
}
