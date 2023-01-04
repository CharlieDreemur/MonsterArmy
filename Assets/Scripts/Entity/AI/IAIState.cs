using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAIState
{
   protected ICharacterAI charAI = null; //角色AI(状态的拥有者)
   public IAIState(){}
   //设置CharacterAI的对象
   public void SetCharacterAI(ICharacterAI CharacterAI){
       charAI = CharacterAI;
   }
   //设置要攻击的目标
    public virtual void SetAttackPosition(Vector3 AttackPosition){}
    
    public virtual void SetAttackTransform(Transform AttackTransform){}
   
    /// <summary>
    /// 检查角色的所有技能，并释放第一个能够释放的技能
    /// </summary>
    public virtual void CheckAbility(){}
    //更新
    public abstract void Update(List<Entity> Targets);
    //目标被删除
    public virtual void RemoveTarget(Entity Target){}

    public Entity FindCloestTarget(List<Entity> Targets){
         //找出最近的目标
        Vector3 NowPosition = charAI.GetPosition();
        Entity theNearTarget = null;
        float MinDist = AllyData.CONST_DETECT_RANGE;
        foreach(Entity Target in Targets){
            //已经阵亡的不计算
            if(Target.IsKilled()){
                continue;
            }
            float dist = Vector3.Distance(NowPosition, Target.GetGameObject().transform.position);
            if(dist < MinDist){
                MinDist = dist;
                theNearTarget = Target;
            }
        }
        return theNearTarget;
    }
}
