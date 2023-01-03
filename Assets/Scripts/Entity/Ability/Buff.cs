using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

/// <summary>
/// Buff的实体,注意 除开nowHP or nowMP是本回合战斗内永久的减少,其他的减少都是有持续时间的,之后会考虑做个工厂模式? 现在先switch case凑合下
/// </summary>
public class Buff : IBuff
{
    [SerializeField]
    private BuffData data;
    
    [SerializeField]
    private ICharacter character;

    private ICharacterAttribute charAttribute;

    [SerializeField]
    private AttributeDataFixed changedfixedData = new AttributeDataFixed(); //该Buff修改的英雄数据，方便回潮

    [SerializeField] 
    public int buffValue; ///buff中变动的数值，比如伤害，治疗量，护盾量，由GameEffect指定

    public Coroutine updateCoroutine; //更新计时器的线程
    public UnityEvent buffEvent; //buff的事件
    public Buff(ICharacter character, BuffData data){
        Init(character,data);
    }
    public void Init(ICharacter _character, BuffData data)
    {
        character = _character;
        this.data = data;
        charAttribute = _character.GetCharacterAttribute();
        changedfixedData = charAttribute.attributeData.fixedData;
    }

    public void SetValue(int value){
        buffValue = value;
    }

    /// <summary>
    /// 启动buff请调用此函数
    /// </summary>
    public void Activate(){
        OnStart();
        updateCoroutine = Tool_Timer.StartCoroutine(Timer_Update(data.durationTime, data.changeInterval));
    }
    public void OnStart()
    {
        changedfixedData = data.fixedData;
        switch (data.buffType)
        {
            case Enum_BuffType.duration:
                switch (data.attributeType)
                {
                    case Enum_AttributeType.add:
                        charAttribute.attributeData.fixedData.Add(data.fixedData);
                        break;
                    case Enum_AttributeType.minus:
                        charAttribute.attributeData.fixedData.Minus(data.fixedData);
                        break;
                }
                break;
            case Enum_BuffType.constantlyChange:
                //Debug.LogWarning("OnStart不支持constantlyChange模式 ");
                break;
        }

    }
    public void OnUpdate()
    {
        //角色死亡时则跳过
        if(character.IsKilled()){
            return;
        }
        VFXUtils.ActivateVFX(data.List_VFX, character.GetPosition());
        switch (data.buffType)
        {
            case Enum_BuffType.duration:
                Debug.LogWarning("Update不支持duration模式 ");
                break;
            case Enum_BuffType.constantlyChange:
                switch (data.attributeType)
                {
                    case Enum_AttributeType.add:
                        charAttribute.attributeData.fixedData.Add(data.fixedData);
                        character.UnderHeal(buffValue);
                        changedfixedData.Add(data.fixedData);
                        break;
                    case Enum_AttributeType.minus:
                        charAttribute.attributeData.fixedData.Minus(data.fixedData);
                        character.UnderHurt(buffValue, Enum_DamageType.ability);
                        changedfixedData.Minus(data.fixedData);
                        break;
                }

                break;
        }

    }
    

    public void OnDestroy()
    {
        //复原除了NowHP和NowMP的属性
        Debug.Log("复原属性");
        switch (data.attributeType)
                {
                    case Enum_AttributeType.add:
                        charAttribute.attributeData.fixedData.Minus(data.fixedData);
                        break;
                    case Enum_AttributeType.minus:
                        charAttribute.attributeData.fixedData.Add(data.fixedData);
                        break;
        }
                
        //销毁
        character.GetCharacterAbility().DeleteBuff(this);
    }

    /// <summary>
    /// 计时到了自动OnDestroy()
    /// </summary>
    /// <value></value>
    IEnumerator Timer_Destroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnDestroy();
    }

    /// <summary>
    /// 在duration秒内，每frequency秒触发一次OnUpdate()
    /// </summary>
    /// <value></value>
    IEnumerator Timer_Update(float duration, float frequency)
    {
        Tool_Timer.StartCoroutine(Timer_Wait(duration));
        while (true)
        {
            yield return new WaitForSeconds(frequency);
            //Debug.Log(data.baseInfo.name+"Update()");
            OnUpdate();
        }
    }

    /// <summary>
    /// 等待duration秒
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator Timer_Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        Tool_Timer.StopCoroutine(ref updateCoroutine);
    }
}



public interface IBuff
{
    public void Init(ICharacter character, BuffData data);

    public void Activate();
    /// <summary>
    /// 
    /// </summary>
    public void OnStart();


    /// <summary>
    ///  buff的更新，比如中毒buff每s扣血
    /// </summary>
    public void OnUpdate();

    /// <summary>
    /// buff的结束，所有的除开nowHP或nowMP的数据都要回溯
    /// </summary>
    public void OnDestroy();
}