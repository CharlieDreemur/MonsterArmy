using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[EnumToggleButtons]
public enum Enum_GameEffectState{
    inactive, //未激活状态，等待被激活
    active, //激活时的状态，激活后变为inactive
    
}
public class GameEffect : IAbility
{
    public GameEffectData gameEffectData;
    public Enum_GameEffectState gameEffectState = Enum_GameEffectState.inactive;
    public ICharacter character;
    private ICharacterAttribute charAttribute;
    private AttributeDataFixed charAttributeData;
    public List<ICharacter> List_Target; //最终技能目标的列表
    [LabelText("技能最终伤害")] [ShowInInspector] [ReadOnly]
    private int finalValue;
    public GameEffect(GameEffectData data, ICharacter character)
    {
        gameEffectData = data; //引用类型,两个值会一起改变
        SetCharacter(character);
        gameEffectState = Enum_GameEffectState.inactive;
        /*#region 值类型，两个data相互独立
        abilityData = ScriptableObject.CreateInstance("AbilityData") as AbilityData;
        abilityData.Init(_abilityData);
        #endregion
        //ps:为什么不使用值类型复制技能数据，是因为我觉得没有这个必要，在大多数情况下，技能数据是不需要改变的，可能只有冷却和法力消耗需要，若有相应的条件，可以把这个注释掉的代码代替上方的
        */
    }

   
    public void SetData(IData data)
    {
        gameEffectData = data as GameEffectData;
    }
    public void SetCharacter(ICharacter _character)
    {
        character = _character;
        charAttribute = _character.GetCharacterAttribute();
        charAttributeData = charAttribute.attributeData.fixedData;
        List_Target = _character.GetCharacterAbility().GetEnemyList();
    }

    public int CalculateAmount(){
        float value = gameEffectData.attributeDataGE.baseValue;
        switch(gameEffectData.attributeDataGE.pctType){
            case Enum_PctAttribute.atk:
                value += charAttributeData.atk.FinalAttribute * gameEffectData.attributeDataGE.pctValue/100f;        
                break;
            case Enum_PctAttribute.ap:
                value += charAttributeData.ap.FinalAttribute * gameEffectData.attributeDataGE.pctValue/100f;
                break;
            case Enum_PctAttribute.hp:
                value += charAttributeData.maxHP.FinalAttribute * gameEffectData.attributeDataGE.pctValue/100f;
                break;
            case Enum_PctAttribute.none:
                break;
        }
        return (int)value;
    }
    public void Init()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
    }

    public Enum_GameEffectState ActivateReturnState(){
        Activate();
        return gameEffectState;

    }

    public void Activate(List<ICharacter> List_Target){
        this.List_Target = List_Target;
        Activate();
    }
    public IEnumerator ActivateDelay(List<ICharacter> List_Target, float seconds){
        this.List_Target = List_Target;
        yield return new WaitForSeconds(seconds);
        Activate();
    }

    public void Activate()
    {   
        if(List_Target == null||List_Target.Count == 0){
            Debug.LogWarning("GameEffect的目标不存在");
        }
        gameEffectState = Enum_GameEffectState.active;
        Debug.Log(charAttribute.CharacterName+"发动了技能"+gameEffectData.baseInfo.name);
        finalValue = CalculateAmount();
        switch (gameEffectData.effectType)
        {
             case Enum_EffectType.damage:
       
                List_Target.ForEach((target)=>{
                     switch(gameEffectData.hurtOrHeal){
                         case Enum_EffectHurtOrHeal.hurt:
                         target.UnderHurt(finalValue, Enum_DamageType.ability);
                             break;
                        case Enum_EffectHurtOrHeal.heal:
                        target.UnderHeal(finalValue);
                            break;
                    }
                    
                 });
            
                 break;
            case Enum_EffectType.buff:
                 gameEffectData.buffData.ForEach((buffData)=>{
                     List_Target.ForEach((target)=>{
                     Buff buff = new Buff(target, buffData);
                   buff.SetValue(finalValue);
                     target.GetCharacterAbility().AddBuff(buff);
                     });
                 });
        
                 break;
             case Enum_EffectType.projectile:
                 Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + "projectile未完成");
                 break;
        }
        Debug.Log("GameEffect"+gameEffectData.name+"执行完毕");
        gameEffectState = Enum_GameEffectState.inactive; //GameEffect Activate完成
    }

    public void Stop()
    {

    }
}
