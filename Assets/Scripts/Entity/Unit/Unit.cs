using System.Net;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MonsterArmy.Core.UnitSystem.Interface;
using MonsterArmy.Core.UnitSystem;
using MonsterArmy.Core;

public interface IUnit
{

}
//继承monobehavior，我们使用一个统一的manager来遍历每个ICharacter
public class Unit : MonoBehaviour, IUnit
{
    //Const
    public const float CONST_DETECT_RANGE = 999f;
    [FoldoutGroup("角色基本信息")]
    public UnitType EntityType
    {
        get
        {
            return AttributeController.UnitType;
        }
    }
    [FoldoutGroup("角色基本资源")]
    public Texture2D Icon;
    [FoldoutGroup("角色基本信息")]
    public bool isKilled = false;
    [ShowInInspector]
    [FoldoutGroup("角色基本信息")]
    protected UnityEngine.GameObject obj;

    [ShowInInspector]
    [FoldoutGroup("角色基本信息")]
    private bool IsInit = false; //是否已经初始化了

    [FoldoutGroup("角色基本信息")]
    [LabelText("角色朝向")]

    public Enum_FaceDirection characterDirection;

    [ShowInInspector]
    [FoldoutGroup("角色基本资源")]
    protected ICharacterAI charAI = null; //AI
    private UnitAttributeController attributeController;
    [ShowInInspector]
    [FoldoutGroup("角色动画资源")]
    protected UnitAnimationController charAnimController = null; //动画控制

    [ShowInInspector]
    [FoldoutGroup("角色基本资源")]
    protected SpriteRenderer[] spriteRenderers; //精灵组

    [ShowInInspector]
    [FoldoutGroup("角色细节数据")]
    protected Vector3 spawnPos = Vector3.zero; //控制spawn坐标

    [FoldoutGroup("角色细节数据")]
    public float HurtTimerLength = 0.3f; //受伤计时器的默认时间长度

    //public static event EventHandler<ProjectileArgs> Event_Shoot;


    [ShowInInspector]
    [FoldoutGroup("角色战斗数据")]
    [InlineProperty]
    [HideLabel]
    protected AbilityController charAbility = null; //角色与技能交互的部分，为了减少单个类的长度我们决定把一些功能放进别的类

    private IAttackComponent attackComponent;

    // /*
    // @Constructor 要创建一个ICharacter的controller，需要一个游戏物体，一个供初始化的角色数据
    // */
    // protected ICharacter(GameObject _obj, ICharacterData _charData){
    // }
    public UnitAttribute Attribute
    {
        get
        {
            return AttributeController.Attribute;
        }
    }
    public UnitAttributeController AttributeController
    {
        get
        {
            if (attributeController == null)
            {
                if (TryGetComponent<UnitAttributeController>(out UnitAttributeController controller))
                {
                    attributeController = controller;
                    return attributeController;
                }
            }
            return attributeController;
        }
    }
    void Awake()
    {
        obj = gameObject;
    }
    public void Update()
    {

        if (!IsInit)
        {
            return;
        }

        if (attributeController.Attribute.HP <= 0 && !IsKilled())
        {
            Killed();

        }
        if (charAI != null)
        {

            charAI.UpdateAI(charAbility.List_Enemy);
        }


    }
    public virtual void Init()
    {
        SetAI(new CharacterAI(this));
        SetCharacterAnimation(GetComponent<UnitAnimationController>());
        SetSpriteRenderers(GetComponentsInChildren<SpriteRenderer>());
        attributeController = GetComponent<UnitAttributeController>();
        SetSpawnPosition(attributeController.SpawnPos);
        GetGameObject().transform.position = GetSpawnPosition();
        attackComponent = GetComponent<RangedAttackComponent>();
        //Const_AttackAnimationLength = UtilsClass.GetAnimatorLength(animator, "Attack");
        tag = "Unit";
        transform.localScale = attributeController.Scale;
        IsInit = true;
    }



    #region Basic

    /*
    @funciton 设定Icharacter出生的坐标
    */
    public void SetSpawnPosition(Vector3 pos)
    {
        spawnPos = pos;
    }

    public void SetCharacterAnimation(UnitAnimationController charAnimation)
    {
        this.charAnimController = charAnimation;
    }

    public void SetSpriteRenderers(SpriteRenderer[] spriteRenderer)
    {
        this.spriteRenderers = spriteRenderer;
    }

    public void SetCharacterAbility(AbilityController charAbility)
    {
        this.charAbility = charAbility;
    }

    public void SetShaderFloat(string parameter, float amount)
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            return;
        }
        foreach (SpriteRenderer item in spriteRenderers)
        {
            item.material.SetFloat(parameter, amount);
        }

    }

    public void SetShaderColor(string parameter, Color color)
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            return;
        }
        foreach (SpriteRenderer item in spriteRenderers)
        {
            item.material.SetColor(parameter, color);
        }
    }


    public void Destroyed()
    {
        UnityEngine.GameObject.Destroy(GetGameObject());
        RemoveGameObject();
    }


    /*
    @function 解除所有其他脚本与该gameobject的绑定
    */
    public void RemoveGameObject()
    {
        SetAI(null);
        SetCharacterAnimation(null);
        SetSpriteRenderers(null);
        SetSpawnPosition(Vector3.zero);
    }

    public UnityEngine.GameObject GetGameObject()
    {
        return obj;
    }

    public string Name
    {
        get
        {
            return Attribute.Name;
        }
    }

    public int ID
    {
        get
        {
            return attributeController.ID;
        }
    }

    public string Description
    {
        get
        {
            return Attribute.Description;
        }
    }

    public DamageInfo GetAttackDamageInfo()
    {
        return new DamageInfo(Attribute.GetAttackDamage(), this);
    }
    public Vector3 GetPosition()
    {
        if (GetGameObject() == null)
        {
            Debug.LogWarning("GetPosition(): GameObject does not exist");
            return Vector3.zero;
        }
        return GetGameObject().transform.position;
    }
    public Transform GetTransform()
    {
        return GetGameObject().transform;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPos;
    }

    public UnitAnimationController GetCharacterAnimation()
    {
        return charAnimController;
    }
    public SpriteRenderer[] GetSpriteRenderers()
    {
        return spriteRenderers;
    }

    public AbilityController GetCharacterAbility()
    {
        return charAbility;
    }


    #endregion

    #region AI
    //设置AI
    public void SetAI(ICharacterAI _characterAI)
    {
        charAI = _characterAI;
    }

    //更新AI
    public void UpdateAI(List<Unit> Targets)
    {
        charAI.UpdateAI(Targets);
    }

    //通知AI有角色被删除
    public void RemoveAITarget(Unit Targets)
    {
        charAI.RemoveAITarget(Targets);
    }
    #endregion

    #region Getter


    public float GetAttackRange()
    {
        return Attribute.AtkRange;
    }

    public float GetAttackInterval()
    {
        return Attribute.AtkInterval;
    }


    public bool IsKilled()
    {
        return isKilled;
    }
    #endregion

    #region BasicFunction
    public void MoveTo(Vector3 targetPos)
    {
        if (Attribute.MoveSpeed <= 0) return;
        FaceEnemy(targetPos);
        charAnimController.PlayAnimationInLength(Enum_AnimationType.Run, Attribute.MoveSpeed);
        //animator.SetInteger("AnimationState", 1);

        GetGameObject().transform.position = Vector3.MoveTowards(GetPosition(), targetPos, Attribute.MoveSpeed * Time.deltaTime);

    }

    public void MoveTo(Transform targetTransform)
    {
        MoveTo(targetTransform.position);
    }

    public void Idle()
    {
        charAnimController.PlayAnimation(Enum_AnimationType.Idle);
        //animator.SetInteger("AnimationState", 0);
    }
    /*
    @function 确保你面向target
    */
    public void FaceEnemy(Vector3 targetPos)
    {
        if (targetPos.x <= GetGameObject().transform.position.x)
        {
            characterDirection = Enum_FaceDirection.left;
            GetGameObject().transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            characterDirection = Enum_FaceDirection.right;
            GetGameObject().transform.rotation = new Quaternion(0, 180, 0, 0);
        }
    }

    public void AttackAnimation(Enum_AttackAnimationType type)
    {
        switch (type)
        {
            case Enum_AttackAnimationType.Sword:
                charAnimController.PlayAnimationInLength(Enum_AnimationType.AttackSword, GetAttackInterval());
                break;
            case Enum_AttackAnimationType.Bow:
                charAnimController.PlayAnimationInLength(Enum_AnimationType.AttackBow, GetAttackInterval());
                break;
            case Enum_AttackAnimationType.Magic:
                charAnimController.PlayAnimationInLength(Enum_AnimationType.AttackMagic, GetAttackInterval());
                break;

        }
    }
    public void TryAttack(Unit target)
    {
        FaceEnemy(target.GetPosition());
        AttackAnimation(Attribute.AttackAnimationType);
        attackComponent?.TryAttack(this, target);
    }
    public void TakeDamage(Unit attacker, int value, Enum_DamageType damageType = Enum_DamageType.PhysicalDamage)
    {
        TakeDamage(new DamageInfo(value, attacker, damageType: damageType));
    }
    public void TakeDamage(DamageInfo damageInfo)
    {
        if (isKilled)
        {
            return;
        }
        attributeController.TakeDamage(damageInfo, out int damage, out bool isDodge, out bool isCrit);
        //闪避了就跳过判断
        if (isDodge)
        {
            Debug.Log(Attribute.Name + " miss the attack");
            //TODO: 闪避特效
            return;
        }
        if (isCrit)
        {
            Debug.Log(damageInfo.attacker.Attribute.Name + " makes a crit damage!");
            //TODO: 暴击特效
        }

        //Play Hurt shader
        StartCoroutine(ShaderChange(HurtTimerLength, Color.white));
        //Create Damage Text
        if (!isDodge)
        {
            string jsonValue = JsonUtility.ToJson(new CreateDamageTextEventArgs(GetPosition(), damage, damageInfo.damageType, isCrit));
            EventManager.TriggerEvent("CreateDamageText", jsonValue);
        }
    }


    private void UnderHeal(int healAmount, bool isCrit = false)
    {
        string jsonValue = JsonUtility.ToJson(new CreateDamageTextEventArgs(GetPosition(), healAmount, Enum_DamageType.Heal, isCrit));
        EventManager.TriggerEvent("CreateDamageText", jsonValue);
        attributeController.UnderHeal(healAmount);
    }

    [Button("Suicide")]
    public void Killed()
    {
        MessageManager.AddMessage(Name + "已死亡");
        isKilled = true;
        SetAI(null);
        StopAllCoroutines();
        StartCoroutine(ShaderChange(HurtTimerLength, Color.red));
        charAnimController.PlayAnimation(Enum_AnimationType.Death);
        StartCoroutine(DelayDeath(charAnimController.DeathAnimationLength));
    }


    #endregion

    #region  Timer
    //在seconds秒中内，反复将shader的_flashAmount减少直到0；
    IEnumerator ShaderChange(float seconds, Color color)
    {
        SetShaderColor("_FlashColor", color);
        //print

        for (float timer = seconds; timer >= 0; timer -= Time.deltaTime)
        {
            SetShaderFloat("_FlashAmount", timer / seconds);
            yield return 0;
        }
    }



    IEnumerator DelayDeath(float seconds)
    {
        UnityEngine.GameObject.Destroy(GetGameObject().GetComponent<Rigidbody2D>());
        yield return new WaitForSeconds(seconds);
        Destroyed();

    }
    #endregion

}

