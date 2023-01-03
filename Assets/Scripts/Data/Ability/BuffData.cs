using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[EnumToggleButtons]
public enum Enum_BuffType{
   duration, ///在一段时间内只发生一次改变，比如减少40%持续3s,或持续4s的眩晕
   constantlyChange, ///在一段时间内，每隔changeInterval秒, 一直改变, 比如每s扣10血，持续5s
   
}

[EnumToggleButtons]
public enum Enum_AttributeType{
   add, ///增加属性
   minus, ///减少属性
}

[Serializable]
[CreateAssetMenu(fileName = "BuffData", menuName = "TheMonsterArmy/AbilitySystem/BuffData", order = 0)]
//BuffData, buff会复制相应data的数据并挂在ICharacter的状态栏(List<Buff>)里
public class BuffData : ScriptableObject
{   
   /// <summary>
   /// 按照data复制一个BuffData
   /// </summary>
   /// <param name="data"></param>
   public BuffData(BuffData data){
      Copy(data);
   }
   public DataBaseInfo baseInfo;

   [FoldoutGroup("Buff基本设置")]
   public Enum_BuffType buffType;

   [FoldoutGroup("Buff基本设置")]
   public Enum_AttributeType attributeType;

   [Tooltip("Buff的持续时间")] [FoldoutGroup("Buff基本设置")]
   public float durationTime;

   [LabelText("特效列表")] 
   public List<VFX> List_VFX = new List<VFX>(); //游戏特效预制体

   [Tooltip("Buff每隔多少秒改变一次属性")] [FoldoutGroup("Buff基本设置")] [ShowIf("buffType", Enum_BuffType.constantlyChange)]
   public float changeInterval;
  
   // DamageValue有四部分的数值，最后一部分作为预览
   // 第一部分为技能的基础伤害/治疗数额
   // 第三部分为技能的属性加成后的结算数值

   public AttributeDataFixed fixedData;
   
   /// <summary>
   /// 按照data的数值初始化
   /// </summary>
   /// <param name="data"></param>
   public void Copy(BuffData data){
      baseInfo = data.baseInfo;
      buffType = data.buffType;
      attributeType = data.attributeType;
      durationTime = data.durationTime;
      List_VFX = data.List_VFX;
      changeInterval = data.changeInterval;
      fixedData = data.fixedData;
   }

}

