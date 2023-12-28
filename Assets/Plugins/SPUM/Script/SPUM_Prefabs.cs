using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPUM_Prefabs : MonoBehaviour
{
    public float _version;
    public SPUM_SpriteList _spriteOBj;
    public bool EditChk;
    public string _code;
    public Animator _anim;

    public bool _horse;
    public string _horseString;


    public void PlayAnimation (int num)
    {
        switch(num)
        {
            case 0: //Idle
            _anim.SetFloat("RunState",0f);
            break;

            case 1: //Run
            _anim.SetFloat("RunState",0.5f);
            break;

            case 2: //Death
            _anim.SetTrigger("Die");
            _anim.SetBool("EditChk",EditChk);
            break;

            case 3: //Stun
            _anim.SetFloat("RunState",1.0f);
            break;

            case 4: //Attack Sword
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",0.0f);
            break;

            case 5: //Attack Bow
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",0.5f);
            break;

            case 6: //Attack Magic
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",1.0f);
            break;

            case 7: //Skill Sword
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",0.0f);
            break;

            case 8: //Skill Bow
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",0.5f);
            break;

            case 9: //Skill Magic
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",1.0f);
            break;
        }
    }
    
    public void PlayAnimation (Enum_AnimationType type)
    {
        switch(type)
        {
            case Enum_AnimationType.Idle: //Idle
            _anim.SetFloat("RunState",0f);
            break;

            case Enum_AnimationType.Run: //Run
            _anim.SetFloat("RunState",0.5f);
            break;

            case Enum_AnimationType.Death: //Death
            _anim.SetTrigger("Die");
            _anim.SetBool("EditChk",EditChk);
            break;

            case Enum_AnimationType.Stun: //Stun
            _anim.SetFloat("RunState",1.0f);
            break;

            case Enum_AnimationType.AttackSword: //Attack Sword
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",0.0f);
            break;

            case Enum_AnimationType.AttackBow: //Attack Bow
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",0.5f);
            break;

            case Enum_AnimationType.AttackMagic: //Attack Magic
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",0.0f);
            _anim.SetFloat("NormalState",1.0f);
            break;

            case Enum_AnimationType.SkillSword: //Skill Sword
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",0.0f);
            break;

            case Enum_AnimationType.SkillBow: //Skill Bow
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",0.5f);
            break;

            case Enum_AnimationType.SkillMagic: //Skill Magic
            _anim.SetTrigger("Attack");
            _anim.SetFloat("AttackState",1.0f);
            _anim.SetFloat("SkillState",1.0f);
            break;
        }
    }
    
}
