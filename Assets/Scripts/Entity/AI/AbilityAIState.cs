using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 释放技能的时候的状态
/// </summary>
public class AbilityAIState : IAIState
{
    private Ability currentAbility;
    private bool isActivate = false;
     
    /// <summary>
    /// 检测CurrentAbilityState是否释放完毕,释放完毕后切换其他State
    /// </summary>
    /// <param name="currentAbility"></param>
    public AbilityAIState(Ability currentAbility){
        this.currentAbility = currentAbility;
        isActivate = false;
        Debug.Log("目前是AbilityAiState");
    }
    public override void Update(List<Entity> Targets)
    {
        
        if(!isActivate){
            currentAbility.Activate(); 
            Debug.Log("目前是AbilityAiState");
            isActivate = true;
        }
        if(currentAbility.abilityState == Enum_AbilityState.cooldown){
            charAI.ChangeAIState(new IdleAIState());
        }
    }
}
