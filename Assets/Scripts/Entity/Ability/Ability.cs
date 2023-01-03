using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

/// <summary>
/// 所以技能模块的实体都可以继承这个
/// </summary>
interface IAbility{
    /// <summary>
    /// 初始化能力
    /// </summary>
    public void Init();
    /// <summary>
    /// 激活能力
    /// </summary>
    public void Activate();

    /// <summary>
    /// 停止能力
    /// </summary>
    public void Stop();

    /// <summary>
    /// 设置数据
    /// </summary>
    public void SetData(IData data);

    /// <summary>
    /// 设置角色
    /// </summary>
    public void SetCharacter(Entity character);
}

[EnumToggleButtons]
public enum Enum_AbilityState{
    ready,
    channel, //技能的蓄力阶段
    active, //技能的激活阶段
    cast, //技能的释放阶段 
    cooldown,
}

[Serializable]
/// <summary>
/// 在战斗开始时，角色会根据abilityData新建相应的ability
/// </summary>
public class Ability : IAbility
{

    public AbilityData abilityData; //abilityData是不变的

    [LabelText("技能拥有者")]
    public Entity character; //技能使用者
    
    public Enum_AbilityState abilityState;

    public Ability(AbilityData data, Entity character){
        Init(data, character);
    }
    
    [LabelText("冷却剩余时间")]
    public float remainTime; //还有多少s冷却结束

    private ICharacterAttribute charAttribute;
    
    private AttributeDataFixed charFixedData;

    
    [LabelText("效果列表")]
    public List<GameEffect> List_GameEffect = new List<GameEffect>();
    [LabelText("目标列表")]
    public List<Entity> List_Target = new List<Entity>(); //技能目标的列表

    [ShowInInspector]
    private Task[] gameEffectTasks;
    private Action callBack = null; //回调函数 TODO:回调事件？UI同步？ 

    [ShowInInspector] [LabelText("正在运行的GameEffect")]
    private int activeGameEffect = 0; //正在运行的GameEffect

    //private AbilityIndicator indicator = null;


    public void Init(AbilityData data, Entity character){
        abilityData = data; //引用类型,两个值会一起改变
        SetCharacter(character);
        Init();
        /*#region 
        值类型，两个data相互独立
        abilityData = ScriptableObject.CreateInstance("AbilityData") as AbilityData;
        abilityData.Init(_abilityData);
        #endregion
        //ps:为什么不使用值类型复制技能数据，是因为我觉得没有这个必要，在大多数情况下，技能数据是不需要改变的，可能只有冷却和法力消耗需要，若有相应的条件，可以把这个注释掉的代码代替上方的
        */ 
    }
    
    /// <summary>
    /// 确保data和character都赋值的情况下调用
    /// </summary>
    public void Init(){
        
        SetGameEffectFromData(abilityData.List_GameEffectData, character);
  
    }

    /// <summary>
    /// 复制AbilityData里的List_VFX
    /// </summary>
    /// <param name="VFXes"></param>
 
    /// <summary>
    /// 根据AbilityData里的GameEffectData实例化GameEffect对象,添加进List_GameEffect里
    /// </summary>
    public void SetGameEffectFromData(List<GameEffectData> gameEffectDatas, Entity character){
        if(gameEffectDatas == null || gameEffectDatas.Count == 0){
            return;
        }
        gameEffectDatas.ForEach((data)=>
        {
            Debug.Log("实例化GameEffect: "+data.baseInfo.name);
            List_GameEffect.Add(new GameEffect(data, character));
           
        });
    }

    /*
    public void SetAbilityIndicator(Ability ability){
        indicator = new AbilityIndicator(ability);
    }
    */

    //返回技能范围
    public float GetAbilityRange(){
        //if attackRange > abiliityRange, then let abilityRange = attackRange;
        if(abilityData.targetChooser.range>=character.GetAttackRange()){
            return abilityData.targetChooser.range;
        }
        else{
            Debug.LogWarning("getAbilityRange()+角色的技能范围小于攻击范围");
            return character.GetAttackRange();
        }
        
    }
    public void SetData(IData data){
        abilityData = data as AbilityData;
    }
    public void SetCharacter(Entity _character){
        character = _character;
        charAttribute = character.GetCharacterAttribute();
        charFixedData = charAttribute.attributeData.fixedData;
    }
    /// <summary>
    /// 触发技能，会将所有List_GameEffect执行
    /// </summary>
    [Button("Activate")]
    public void Activate(){
        //技能列表检查
        if(abilityData.List_GameEffectData == null || abilityData.List_GameEffectData.Count==0){
            return;
        }
        //冷却,法力检查
        if(!CheckTime() || !CheckCost()){
            return;
        }

        List_Target = ChooseTarget();
        //距离检查
        if(!CheckDistance(List_Target[0])){
            return;
        }
        AbilityCost();
        abilityState = Enum_AbilityState.channel;
        foreach(Entity target in List_Target){
            MessageBoard.Instance.AddMessage(character.Name+"对"+target.Name+"释放了"+abilityData.baseInfo.name);
        }
        //播放角色释放技能的动画
        character.GetCharacterAnimation().PlayAnimationInLength(abilityData.animationType, abilityData.channelTime);
        //对于引导技能，延时调用
        Tool_Timer.DelayToInvokeBySecond(CastAbility, abilityData.channelTime);
        
    }

    /// <summary>
    /// 选定目标的activate
    /// </summary>
    /// <param name="targets"></param>
    public void Activate(List<Entity> targets){
        //技能列表检查
        if(abilityData.List_GameEffectData == null || abilityData.List_GameEffectData.Count==0){
            return;
        }
        //冷却,法力检查
        if(!CheckTime() || !CheckCost()){
            return;
        }

        List_Target = targets;
        //距离检查
        if(!CheckDistance(List_Target[0])){
            return;
        }
        abilityState = Enum_AbilityState.channel;
        MessageBoard.Instance.AddMessage(character.Name+"对"+List_Target[0].Name+"释放了"+abilityData.baseInfo.name);
        //播放角色释放技能的动画
        character.GetCharacterAnimation().PlayAnimationInLength(abilityData.animationType, abilityData.channelTime);
        //对于引导技能，延时调用
        Tool_Timer.DelayToInvokeBySecond(CastAbility, abilityData.channelTime);
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    public void CastAbility(){
        abilityState = Enum_AbilityState.cast;
        activeGameEffect = List_GameEffect.Count;

        //释放技能特效
        VFXUtils.ActivateVFX(abilityData.List_VFX, character.GetPosition(), List_Target);
        for(int i=0; i<List_GameEffect.Count;i++){
            GameEffect effect= List_GameEffect[i];
            Coroutine callback= Tool_Timer.StartCoroutine(effect.ActivateDelay(List_Target, 0f));
            //等待所有Effect Activate完毕
            Tool_Timer.StartCoroutine(CheckIsFinshed(callback));
            
        }
    }
    /// <summary>
    /// 检查所有GameEffect是否运行完成
    /// </summary>
    /// <param name="gameEffectCallBack"></param>
    /// <returns></returns>
    private IEnumerator CheckIsFinshed(Coroutine gameEffectCallBack){
            yield return gameEffectCallBack;
            activeGameEffect -= 1;
            if(activeGameEffect == 0){
                abilityState = Enum_AbilityState.cooldown;
                //启动冷却计时器
                Tool_Timer.StartCoroutine(Ability_Timer(abilityData.cooldown));
            }
    }

 
    /// <summary>
    /// 检查技能是否可以对目标释放，必须先传递目标，否则return false
    /// </summary>
    /// <returns></returns>
    public bool CheckAbility(Entity target){
        if(CheckTime()&&CheckCost()){
            //如果没有距离限制则不需要检测距离
            if(abilityData.targetChooser.isRangeInfinity){
                return true;
            }
            else{
                return CheckDistance(target);
            }
            
        }
        else{
            return false;
        }
    }
    
    private bool CheckTime(){
        if(abilityState == Enum_AbilityState.ready){
            return true;
        }
        else{
            //Debug.Log(abilityData.baseInfo.name+"还有"+remainTime+"秒冷却完毕");
            return false;
        }
    }
    /// <summary>
    /// 检查相应的技能消耗
    /// </summary>
    /// <returns></returns>
    private bool CheckCost(){
        switch(abilityData.costType){
            case Enum_CostType.none:
                return true;

            case Enum_CostType.mp:
            if(charAttribute.attributeData.NowMP >= abilityData.costValue){
                
                return true;
            }
            else{
                //Debug.Log(abilityData.baseInfo.name+"的当前法力值满足不了技能花费（"+abilityData.costValue+"）");
            }
            break;

            case (Enum_CostType.hp):
            if(charAttribute.attributeData.NowHP >= abilityData.costValue){
                return true;
            }
            else{
                //Debug.Log(abilityData.baseInfo.name+"的当前生命值满足不了技能花费（"+abilityData.costValue+"）");
            }
            break;
            default:
            Debug.LogWarning("CheckCost()未知的Enum_CostType");
            break;
        }
        return false;   
    }

    private void AbilityCost(){
        switch(abilityData.costType){
            case Enum_CostType.none:
            break;

            case Enum_CostType.mp:
            if(charAttribute.attributeData.NowMP >= abilityData.costValue){
                charAttribute.attributeData.NowMP -= abilityData.costValue;
            
            }

            break;

            case (Enum_CostType.hp):
            if(charAttribute.attributeData.NowHP >= abilityData.costValue){
                charAttribute.attributeData.NowHP -= abilityData.costValue;
            }
           
            break;
            default:
            Debug.LogWarning("CheckCost()未知的Enum_CostType");
            break;
        }
    }
    /// <summary>
    /// 检查目标是否在技能的释放范围内
    /// ps:我们认为技能目标为多数（multi）的技能，只需要有一个目标满足即可
    /// </summary>
    /// <returns></returns>
    private bool CheckDistance(Entity target){
        if(Vector3.Distance(target.GetPosition(), character.GetPosition()) <= GetAbilityRange()){
            return true;
        }
        else{
            //Debug.Log(target.Name+"不在"+ character.Name+"的技能范围内（"+abilityData.targetChooser.range+")");
            return false;
        }
    }
    public virtual void Stop(){
        ///TODO:中断技能释放
    }

    public List<Entity> ChooseTarget()
    {
        return UtilsTargetChooser.ChooseTarget(abilityData.targetChooser, character, character.GetCharacterAbility().GetEnemyList(), character.GetCharacterAbility().GetFriendList());
    }
    
    

  

    
    /// <summary>
    /// 技能冷却计时器, 精准到0.01s，冷却完毕后会更改State到ready并调用相应的回调函数
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    IEnumerator Ability_Timer(float seconds){
        if(seconds <=0){
            remainTime=0f;
            yield break;
        }
        remainTime = seconds;
        while(remainTime >= 0.1f){
            yield return new WaitForSeconds(0.1f);
            remainTime -= 0.1f;
        }
        remainTime = 0f;
        abilityState = Enum_AbilityState.ready;
        if(callBack!=null){
            callBack.Invoke(); //调用回调函数
        }
        
    }

 


}

