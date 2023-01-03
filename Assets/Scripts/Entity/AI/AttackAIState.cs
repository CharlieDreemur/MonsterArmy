using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAIState : IAIState
{
    [SerializeField]
    private ICharacter attackTarget = null; //攻击目标
    public AttackAIState(ICharacter _attackTarget){
        attackTarget = _attackTarget;
    }
    //更新
    public override void Update(List<ICharacter> Targets){
    
        //没有目标时，改为Idle
        if(attackTarget == null || attackTarget.IsKilled()||Targets ==null || Targets.Count ==0){
            charAI.ChangeAIState(new IdleAIState());
            return;
        }
        
        Ability ability = charAI.CheckAbility(attackTarget);
        //如果有任何可以释放的技能，则切换为释放技能
        if(ability!=null){
            charAI.ChangeAIState(new AbilityAIState(ability));
            return;
        }

        //不在攻击范围内，改为追击
        if(charAI.IsTargetInAttackRange(attackTarget) == false){
            charAI.ChangeAIState(new ChaseAIState(attackTarget));
            return;
        }
        if(!charAI.attackable){
           return;
       }
        //攻击
        charAI.Attack(attackTarget);

    }
    //目标被删除
    public override void RemoveTarget(ICharacter Target)
    {
        if(attackTarget.GetGameObject().name == Target.GetGameObject().name)
            attackTarget = null;
    }

}
