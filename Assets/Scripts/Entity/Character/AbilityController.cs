using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

/// <summary>
/// 负责AbilitySystem相关的ICharacter逻辑
/// </summary>
[HideLabel] [System.Serializable] [InlineProperty] 
public class AbilityController
{
    [ReadOnly]
    public Entity character;

    [ShowInInspector] [LabelText("敌人列表")] 
    public List<Entity> List_Enemy = null;

    [ShowInInspector] [LabelText("友方列表")]
    public List<Entity> List_Friend = null;

    [ShowInInspector] [LabelText("Buff列表")] 

    public List<Buff> List_Buff = new List<Buff>(); //关于buff的List
    
    [ShowInInspector] [LabelText("技能列表")]
    public List<Ability> List_Ability = new List<Ability>(); //关于Ability的List
    

    public AbilityController(Entity character, List<Entity> List_Enemy,  List<Entity> List_Friend){
        SetCharacter(character);
        SetEnemyList(List_Enemy);
        SetFriendList(List_Friend);
    }

    public void UpdateTargets(List<Entity> List_Enemy,  List<Entity> List_Friend){
        SetEnemyList(List_Enemy);
        SetFriendList(List_Friend);
    }


    public void SetCharacter(Entity character){
        this.character = character;
    }
    /// <summary>
    /// 设置Enemy目标列表
    /// </summary>
    /// <param name="List_Enemy"></param>
    public void SetEnemyList(List<Entity> List_Enemy)
    {
        this.List_Enemy = List_Enemy;
    }

    /// <summary>
    /// 获取Enemy目标列表
    /// </summary>
    /// <returns></returns>
    public List<Entity> GetEnemyList()
    {
        return this.List_Enemy;
    }

    /// <summary>
    /// 设置Friend目标列表
    /// </summary>
    /// <param name="List_Friend"></param>
    public void SetFriendList(List<Entity> List_Friend)
    {
        this.List_Friend = List_Friend;
    }

    /// <summary>
    /// 获取Friend目标列表
    /// </summary>
    /// <returns></returns>
    public List<Entity> GetFriendList()
    {
        return this.List_Friend;
    }

    /// <summary>
    /// 给相应的ICharacter的Buff栏添加buff
    /// </summary>
    public void AddBuff(Buff buff){ 
        List_Buff.Add(buff);
        buff.Activate();
    }

    /// <summary>
    /// 删除Buff栏里的buff
    /// </summary>
    public void DeleteBuff(Buff buff){
        List_Buff.Remove(buff);
    }

    /// <summary>
    /// 清空BUff栏里的所有Buff
    /// </summary>
    public void ClearBuff(){
        List_Buff = new List<Buff>();
    }
}