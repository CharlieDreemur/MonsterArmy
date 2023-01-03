using System;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//AI状态的拥有者
public abstract class ICharacterAI 
{
  
    protected Entity character = null;
    [ShowInInspector]
    protected IAIState AIState = null; //角色AI状态

    public float attackIntervalTimer = 0f;
    public bool attackable = true; //是否可以攻击，false则角色无法攻击
    public ICharacterAI(Entity _character){
        character = _character;
        //首次出手无前摇
        attackIntervalTimer = 0f;
    }

   //更换AI状态
    public virtual void ChangeAIState(IAIState NewAIState){
       //Debug.Log(character.GetGameObject().name+" change into "+NewAIState);
       AIState = NewAIState;
       AIState.SetCharacterAI(this);
    }

   //攻击目标
    public virtual void Attack(Entity Target){
    
       //时间到了再攻击
       attackIntervalTimer -= Time.deltaTime;
       if(attackIntervalTimer > 0){
           return;
       }
       attackIntervalTimer = character.GetAttackInterval();
       //攻击目标
       switch (character.GetCharacterData().attackType){
            case Enum_AttackType.attack:
            character.Attack(Target);
            break;
            case Enum_AttackType.rangedAttack:
            character.RangedAttack(Target);
            break;

            default:
            Debug.LogWarning("未知的攻击类型");
            break;

       }
    
   }

    //是否在攻击距离内
    public bool IsTargetInAttackRange(Entity Target){
        float dist = Vector3.Distance(character.GetPosition(), Target.GetPosition());
        return (dist<=character.GetAttackRange());
    }

    //当前的位置
    public Vector3 GetPosition(){
        return character.GetPosition();
    }

    //移动
    public void MoveTo(Vector3 _pos){
        character.MoveTo(_pos);
    }

    public void MoveTo(Transform _transform){
        character.MoveTo(_transform);
    }
   //停止移动
    public void Idle(){
        character.Idle();
    }

  
    //设置阵亡
    public void Killed(){
       character.Killed();
    } 

   //是否阵亡
    public bool IsKilled(){
       return character.IsKilled();
    }

   //目标删除
    public void RemoveAITarget(Entity Target){
       AIState.RemoveTarget(Target);
    }


    #region 技能条件相关
    /// <summary>
    /// Activate所有Ready的Ability
    /// </summary>
    public void ActivateAllReadyAbility(){
        foreach(Ability ability in character.GetCharacterAbility().List_Ability){
            ability.Activate();
        }
    }
    
    /// <summary>
    /// 检测List_Ability中的所有技能，返回一个包含所有可以使用的技能的List，如果找不到则返回null
    /// </summary>
    public List<Ability> GetReadyAbilities(Entity target){
        List<Ability> List_ReturnAbility = null;
        foreach(Ability ability in character.GetCharacterAbility().List_Ability){
            if(ability.CheckAbility(target)){
                List_ReturnAbility.Add(ability);
            }
        }
        return List_ReturnAbility;
    }

   

    /// <summary>
    /// 检测List_Ability中的所有技能，返回第一个可以使用的技能，如果找不到则返回null
    /// </summary>
    public Ability CheckAbility(Entity target){
        foreach(Ability ability in character.GetCharacterAbility().List_Ability){
            if(ability.CheckAbility(target)){
                return ability;
            }
        }
         return null;
    }

    public void ActivateReadyAbilityInInterval(Entity target){
         //技能每60帧执行一次,没有技能不执行
        if(Time.frameCount % 60 != 0 || character.GetCharacterAbility().List_Ability.Count==0 || character.GetCharacterAbility().List_Ability == null){
            return;
        }
        ActivateReadyAbility(target);
    }


    public void ActivateReadyAbility(Entity target){
    
        //有能够释放的技能时，先释放技能
            Ability ability = CheckAbility(target);
            if(ability!=null){
            ability.Activate();
            }
    }

    #endregion
    
    //更新AI
    public void UpdateAI(List<Entity> Targets){

       AIState.Update(Targets);
    }


   //是否可以攻击Heart
    public abstract bool CanAttackHeart();
}

