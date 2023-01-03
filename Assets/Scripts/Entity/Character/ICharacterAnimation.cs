using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



/// <summary>
/// 帮助ICharacter控制动画相关
/// </summary>
[InlineProperty] [System.Serializable] [HideLabel] 
public class ICharacterAnimation : MonoBehaviour{
    public Animator animator; //动画控制
    public float RunAnimationLength;
    public const float CONST_BASE_MOVESPEED = 10.0f;

    public float AttackSowrdAnimationLength;//攻击动画的时长（提前存储减少卡顿） 
    public float AttackBowAnimationLength;
    public float AttackMagicAnimationLength;
    public float SkillSowrdAnimationLength;
    public float SkillBowAnimationLength;
    public float SkillMagicAnimationLength;

    public float DeathAnimationLength; //死亡动画时长

    public float moveSpeed;
    public float attackSpeed;

    public float abilitySpeed;

    [TableList]
    public AnimatorClipInfo[] animationClip;

    public void Awake(){
        animator = transform.GetChild(0).GetComponent<Animator>();
        RunAnimationLength = UtilsClass.GetAnimatorLength(animator, "1_Run");
        AttackSowrdAnimationLength = UtilsClass.GetAnimatorLength(animator, "2_Attack_Normal");
        AttackBowAnimationLength = UtilsClass.GetAnimatorLength(animator, "2_Attack_Bow");
        AttackMagicAnimationLength = UtilsClass.GetAnimatorLength(animator, "2_Attack_Magic");
        SkillSowrdAnimationLength = UtilsClass.GetAnimatorLength(animator, "5_Skill_Normal");
        SkillBowAnimationLength = UtilsClass.GetAnimatorLength(animator, "5_Skill_Bow");
        SkillMagicAnimationLength = UtilsClass.GetAnimatorLength(animator, "5_Skill_Magic");
        animationClip = animator.GetCurrentAnimatorClipInfo(0);
        DeathAnimationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        if(AttackSowrdAnimationLength == 0){
            Debug.LogWarning("Const_AttackAnimationLength不能为0");
        }
    }
    /*
    public void PlayAnimationInLength(Enum_AnimationType type){
        if(type == Enum_AnimationType.Run){
            moveSpeed = character.GetCharacterAttribute().attributeData.fixedData.MoveSpeed;
            animator.SetFloat("MoveSpeed", moveSpeed*0.1f);
            PlayAnimation(type);
            return;
        }
        
        if(type == Enum_AnimationType.AttackSword || type ==Enum_AnimationType.AttackBow || type ==Enum_AnimationType.AttackMagic){
            PlayAnimationInLength(type, character.GetCharacterAttribute().attributeData.fixedData.AtkInterval);
        }
        else{
            animator.SetFloat("MoveSpeed", 1f);
            animator.SetFloat("AttackSpeed", 1f);
            PlayAnimation(type);
        }
        

    }
    */
  

        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="length">动画需要持续的时长</param>
    public void PlayAnimationInLength(Enum_AnimationType type, float length){
        switch(type){
            case Enum_AnimationType.Run:
            animator.SetFloat("MoveSpeed", length/CONST_BASE_MOVESPEED);
            //Debug.Log("MoveSpeed: "+length/CONST_BASE_MOVESPEED);
            break;
            case Enum_AnimationType.AttackSword:
            attackSpeed = AttackSowrdAnimationLength / length;
            animator.SetFloat("AttackSpeed", attackSpeed);
            break;
            case Enum_AnimationType.AttackBow:
            attackSpeed = AttackBowAnimationLength / length;
            animator.SetFloat("AttackSpeed", attackSpeed);
            break;
            case Enum_AnimationType.AttackMagic:
            attackSpeed = AttackMagicAnimationLength / length;
            animator.SetFloat("AttackSpeed", attackSpeed);
            break;
            
            case Enum_AnimationType.SkillSword:
            abilitySpeed = SkillSowrdAnimationLength /length;
            animator.SetFloat("AttackSpeed", abilitySpeed);
            break;

            case Enum_AnimationType.SkillBow:
            abilitySpeed = SkillBowAnimationLength /length;
            animator.SetFloat("AttackSpeed", abilitySpeed);
            break;

            case Enum_AnimationType.SkillMagic:
            abilitySpeed = SkillMagicAnimationLength /length;
            animator.SetFloat("AttackSpeed", abilitySpeed);
            break;

            default:
            animator.SetFloat("MoveSpeed", 1f);
            animator.SetFloat("AttackSpeed", 1f);
            break;
        }

        PlayAnimation(type);
    }
    
    public void PlayAnimation (Enum_AnimationType type)
    {
        switch(type)
        {
            case Enum_AnimationType.Idle: //Idle
            animator.SetFloat("RunState",0f);
            break;

            case Enum_AnimationType.Run: //Run
            animator.SetFloat("RunState",0.5f);
            break;

            case Enum_AnimationType.Stun: //Stun
            animator.SetFloat("RunState",1.0f);
            break;

            case Enum_AnimationType.Death: //Death
            animator.SetTrigger("Die");
            break;

            case Enum_AnimationType.AttackSword: //Attack Sword
            animator.SetTrigger("Attack");
            animator.SetFloat("AttackState",0.0f);
            animator.SetFloat("NormalState",0.0f);
            break;

            case Enum_AnimationType.AttackBow: //Attack Bow
            animator.SetTrigger("Attack");
            animator.SetFloat("AttackState",0.0f);
            animator.SetFloat("NormalState",0.5f);
            break;

            case Enum_AnimationType.AttackMagic: //Attack Magic
            animator.SetTrigger("Attack");
            animator.SetFloat("AttackState",0.0f);
            animator.SetFloat("NormalState",1.0f);
            break;

            case Enum_AnimationType.SkillSword: //Skill Sword
            animator.SetTrigger("Attack");
            animator.SetFloat("AttackState",1.0f);
            animator.SetFloat("SkillState",0.0f);
            break;

            case Enum_AnimationType.SkillBow: //Skill Bow
            animator.SetTrigger("Attack");
            animator.SetFloat("AttackState",1.0f);
            animator.SetFloat("SkillState",0.5f);
            break;

            case Enum_AnimationType.SkillMagic: //Skill Magic
            animator.SetTrigger("Attack");
            animator.SetFloat("AttackState",1.0f);
            animator.SetFloat("SkillState",1.0f);
            break;
        }
    }
    
}
