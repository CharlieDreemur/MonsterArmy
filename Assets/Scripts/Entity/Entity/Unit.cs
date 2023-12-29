using System.Net;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MonsterArmy.Core.UnitSystem.Components;

//继承monobehavior，我们使用一个统一的manager来遍历每个ICharacter
public abstract class Unit : MonoBehaviour
{
    [FoldoutGroup("角色基本信息")]
    public EntityData data;
    public UnitType entityType = UnitType.None;
    [FoldoutGroup("角色基本资源")]
    public Texture2D Icon;
    [FoldoutGroup("角色基本信息")]
    public bool isKilled = false;
    [ShowInInspector] [FoldoutGroup("角色基本信息")]
    protected UnityEngine.GameObject obj;
    
    [ShowInInspector] [FoldoutGroup("角色基本信息")]
    private bool IsInit = false; //是否已经初始化了

    [FoldoutGroup("角色基本信息")] [LabelText("角色朝向")]

    public Enum_FaceDirection characterDirection;

    [ShowInInspector] [FoldoutGroup("角色基本资源")]
    protected ICharacterAI charAI = null; //AI
    [ShowInInspector] [FoldoutGroup("角色动画资源")]
    protected AnimationController charAnimController = null; //动画控制
    
    [ShowInInspector] [FoldoutGroup("角色基本资源")]
    protected SpriteRenderer[] spriteRenderers; //精灵组

    [ShowInInspector] [FoldoutGroup("角色细节数据")]
    protected EntityAttribute charAttr = null; //数据计算(数据变量)
    [ShowInInspector] [FoldoutGroup("角色细节数据")]
    protected Vector3 spawnPos = Vector3.zero; //控制spawn坐标

    [FoldoutGroup("角色细节数据")]
    public float HurtTimerLength = 0.3f; //受伤计时器的默认时间长度

    //public static event EventHandler<ProjectileArgs> Event_Shoot;


    [ShowInInspector] [FoldoutGroup("角色战斗数据")] [InlineProperty] [HideLabel]
    protected AbilityController charAbility = null; //角色与技能交互的部分，为了减少单个类的长度我们决定把一些功能放进别的类

    private IAttackComponent attackComponent;

    // /*
    // @Constructor 要创建一个ICharacter的controller，需要一个游戏物体，一个供初始化的角色数据
    // */
    // protected ICharacter(GameObject _obj, ICharacterData _charData){
    // }

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

        if (charAttr.attributeData.NowHP <= 0 && !IsKilled())
        {
            Killed();

        }
        if (charAI != null)
        {
           
            charAI.UpdateAI(charAbility.List_Enemy);
        }
    

    }
    public virtual void Init(EntityData data)
    {
        GetCharacterAttribute().Init(data, this);
        SetAI(new CharacterAI(this));
        SetAbilityFromData();
        SetCharacterAnimation(GetComponent<AnimationController>());
        SetSpriteRenderers(GetComponentsInChildren<SpriteRenderer>());
        SetSpawnPosition(charAttr.Transform_SpawnPos);
        GetGameObject().transform.position = GetSpawnPosition();
        attackComponent = GetComponent<RangedAttackComponent>();
        //Const_AttackAnimationLength = UtilsClass.GetAnimatorLength(animator, "Attack");
        tag = "Unit";
        transform.localScale = charAttr.Scale;
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

    public void SetCharacterAttribute(EntityAttribute charAttr)
    {
        this.charAttr = charAttr;
    }

    public void SetAbility(List<Ability> List_Ability)
    {
        charAbility.List_Ability = List_Ability;
    }

    public void SetAbilityFromData()
    {
        SetAbilityFromData(GetCharacterAttribute().List_AbilityData, this);
    }

    public void SetAbilityFromData(List<AbilityData> abilityDatas, Unit character)
    {

        if (abilityDatas == null || abilityDatas.Count == 0)
        {
            return;
        }

        abilityDatas.ForEach((data) =>
        {
            if(data == null){
                Debug.LogWarning("the data of the character's abilityList is Null"+character.Name);
                return;
            }
            //Debug.Log(data.baseInfo.name);
            charAbility.List_Ability.Add(new Ability(data, character));

        });
    }

    public void SetCharacterAnimation(AnimationController charAnimation){
        this.charAnimController = charAnimation;
    }
    
    public void SetSpriteRenderers(SpriteRenderer[] spriteRenderer)
    {
        this.spriteRenderers = spriteRenderer;
    }

    public void SetCharacterAbility(AbilityController charAbility){
        this.charAbility = charAbility;
    }

    public void SetShaderFloat(string parameter, float amount)
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0) 
        {
            return;
        }
        foreach(SpriteRenderer item in spriteRenderers){
            item.material.SetFloat(parameter, amount);
        }
        
    }

    public void SetShaderColor(string parameter, Color color)
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0) 
        {
            return;
        }
        foreach(SpriteRenderer item in spriteRenderers){
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

    public string Name{
        get{
            return GetCharacterData().baseInfo.name;
        }
    }

    public string ID{
         get{
            return GetCharacterData().baseInfo.name;
        }
    }

    public string Description{
        get{
            return GetCharacterData().baseInfo.description;
        }
    }
    public Vector3 GetPosition()
    {
        if(GetGameObject()==null){
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
   
    public AnimationController GetCharacterAnimation(){
        return charAnimController;
    }
    public SpriteRenderer[] GetSpriteRenderers()
    {
        return spriteRenderers;
    }

    public EntityData GetCharacterData()
    {
        return charAttr.data;
    }


    public EntityAttribute GetCharacterAttribute()
    {
        return charAttr;
    }

 
    public AbilityController GetCharacterAbility(){
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
        return charAttr.attributeData.fixedData.AtkRange;
    }

    public float GetAttackInterval()
    {
        return charAttr.attributeData.fixedData.AtkInterval;
    }


    public bool IsKilled()
    {
        return isKilled;
    }
    #endregion

    #region BasicFunction
    public void MoveTo(Vector3 targetPos)
    {
        if (charAttr.attributeData.fixedData.MoveSpeed <= 0) return;
        FaceEnemy(targetPos);
        charAnimController.PlayAnimationInLength(Enum_AnimationType.Run, GetCharacterAttribute().attributeData.fixedData.MoveSpeed);
        //animator.SetInteger("AnimationState", 1);
    
        GetGameObject().transform.position = Vector3.MoveTowards(GetPosition(), targetPos, charAttr.attributeData.fixedData.MoveSpeed * Time.deltaTime);

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

    public void AttackAnimation(Enum_AttackAnimationType type){
        switch(type){
            case Enum_AttackAnimationType.Sword:
                charAnimController.PlayAnimationInLength(Enum_AnimationType.AttackSword, GetAttackInterval());
                break;
            case Enum_AttackAnimationType.Bow:
                charAnimController.PlayAnimationInLength(Enum_AnimationType.AttackBow, GetAttackInterval());
                break;
            case Enum_AttackAnimationType.Magic:
                charAnimController.PlayAnimationInLength(Enum_AnimationType.AttackMagic,GetAttackInterval());
                break;

        }
    }
    public void AttackUtils(Unit target){
        FaceEnemy(target.GetPosition());
        AttackAnimation(GetCharacterData().attackAnimationType);
    }
    public void Attack(Unit target)
    {   
        AttackUtils(target);
        DamageInfo damageInfo = charAttr.GetAttackDamageInfo();
        StartCoroutine(DelayAttack(charAttr.attributeData.fixedData.AtkInterval*0.6f, damageInfo, target));
    }

    public void RangedAttack(Unit target)
    {   
        AttackUtils(target);
        attackComponent?.TryAttack(this, target);

    }

    public void TakeDamage(DamageInfo damageInfo){
        if(isKilled){
            return;
        }
        int atkDamage = EntityAttribute.CalculateDamage(damageInfo.attacker, this, out bool IsDodge, out Enum_DamageType damageType);

        // 伤害不能为负数
        if (atkDamage < 0)
        {
            atkDamage = 0;
        }

        //闪避了就跳过判断
        if (IsDodge)
        {
            Debug.Log(this.GetCharacterAttribute().CharacterName + "闪避了");
            //TODO: 闪避特效
            return;
        }
        if (damageType == Enum_DamageType.crit)
        {
            Debug.Log(damageInfo.attacker.GetCharacterAttribute().CharacterName + "闪避了");
            //TODO: 暴击特效
        }

        //受伤特效shader
        StartCoroutine(ShaderChange(HurtTimerLength, Color.white));
        UnderHurt(atkDamage, damageType);
    }

  
    public void UnderHurt(int damageAmount, Enum_DamageType damageType){
        string jsonValue = JsonUtility.ToJson(new CreateDamageTextEventArgs(GetPosition(), damageAmount, damageType));
        EventManager.TriggerEvent("CreateDamageText", jsonValue);
        UnderHurt(damageAmount);
    }
    
    public void UnderHurt(int damage){
        charAttr.UnderHurt(damage);
    }

    public void UnderHeal(int healAmount){
        string jsonValue = JsonUtility.ToJson(new CreateDamageTextEventArgs(GetPosition(), healAmount, Enum_DamageType.heal));
        EventManager.TriggerEvent("CreateDamageText", jsonValue);
        charAttr.UnderHeal(healAmount);
    }

    [Button("Suicide")] 
    public void Killed()
    {
        MessageManager.AddMessage(Name+"已死亡");
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


    
    IEnumerator DelayAttack(float seconds, DamageInfo damageInfo, Unit target)
    {
        yield return new WaitForSeconds(seconds);
        //Debug.Log("AttackWait"+Time.time);
        target.TakeDamage(damageInfo);
    }

    IEnumerator DelayDeath(float seconds)
    {
        UnityEngine.GameObject.Destroy(GetGameObject().GetComponent<Rigidbody2D>());
        yield return new WaitForSeconds(seconds);
        Destroyed();

    }
    #endregion
    
}

