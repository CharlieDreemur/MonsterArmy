using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAIState : IAIState
{
    private ICharacter chaseTarget = null; //追击的目标
    private const float CHASE_CHECK_DIST = 0.2f; //TODO:chase_check_dist?
    private Vector3 chasePosition = Vector3.zero;
    private Transform chaseTransform = null;
    private bool b_OnChase = false;
    public ChaseAIState(ICharacter _chaseTarget){
        chaseTarget = _chaseTarget;
    }
    //更新
    public override void Update(List<ICharacter> Targets) {
        //没有目标时，改为待机
        if(chaseTarget == null || chaseTarget.IsKilled()){
            charAI.ChangeAIState(new IdleAIState());
            return;
        }
        //在攻击范围内，改为攻击
        if(charAI.IsTargetInAttackRange(chaseTarget)){
            charAI.Idle();
            charAI.ChangeAIState(new AttackAIState(chaseTarget));
            return;
        }
        
        //已经在追击
        if(b_OnChase){
            //已到达追击位置，但目标不见，改为待机
            //float dist = Vector3.Distance(chasePosition, charAI.GetPosition());
            float dist = Vector3.Distance(chaseTransform.position, charAI.GetPosition());
            if(dist<CHASE_CHECK_DIST){
                charAI.ChangeAIState(new IdleAIState());
                return;
            }
            else{
                //定时搜索更近的目标
                ICharacter target = FindCloestTarget(Targets);
                if(target!=chaseTarget){
                    chaseTarget = target;
                }
            }
           
        }
        Ability ability = charAI.CheckAbility(chaseTarget);
        //如果有任何可以释放的技能，则切换为释放技能
        if(ability!=null){
            charAI.ChangeAIState(new AbilityAIState(ability));
            return;
        }
      
        //往目标移动
        b_OnChase = true;
        //chasePosition = chaseTarget.GetPosition();
        chaseTransform = chaseTarget.GetTransform();
        //charAI.MoveTo(chasePosition);
        charAI.MoveTo(chaseTransform);
    }

    // 目標被移除
	public override void RemoveTarget(ICharacter Target)
	{
		if( chaseTarget.GetGameObject().name == Target.GetGameObject().name )
			chaseTarget = null;
	}
}
