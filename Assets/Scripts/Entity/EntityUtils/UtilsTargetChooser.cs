using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

/// <summary>
/// 目标选择器
/// </summary>
public static class UtilsTargetChooser
{
    /// <summary>
    /// Target选择器，按照条件选择，返回List<ICharacter>，用List<ICharacter>[0]来获取单体目标
    /// </summary>
    /// <returns></returns>
    public static List<Unit> ChooseTarget(TargetChooserField chooser, Unit character, List<Unit> enemys, List<Unit> allies) 
    {
        List<Unit> List_Return = new List<Unit>();
        switch (chooser.effectTargetChooser)
        {
            case Enum_EffectTargetChooser.current:
                break;
            case Enum_EffectTargetChooser.cloest:
                switch (chooser.effectTargetType)
                {
                    case Enum_EffectTargetType.enemy:
                        List_Return = UtilsTargetChooser.FindClosest(character, enemys, chooser.targetNumber,  chooser.isTargetAll,  chooser.isRangeInfinity,  chooser.range);
                        break;

                    case Enum_EffectTargetType.ally:
                        List_Return = UtilsTargetChooser.FindClosest(character, allies,  chooser.targetNumber,  chooser.isTargetAll,  chooser.isRangeInfinity,  chooser.range);
                        break;
                }

                break;
            case Enum_EffectTargetChooser.farwest:
                switch (chooser.effectTargetType)
                {
                    case Enum_EffectTargetType.enemy:
                        List_Return = UtilsTargetChooser.FindFarest(character, enemys,  chooser.targetNumber,  chooser.isTargetAll,  chooser.isRangeInfinity,  chooser.range);
                        break;
                    case Enum_EffectTargetType.ally:
                        List_Return = UtilsTargetChooser.FindFarest(character, allies,  chooser.targetNumber,  chooser.isTargetAll,  chooser.isRangeInfinity,  chooser.range);
                        break;
                }

                break;
            case Enum_EffectTargetChooser.leastHP:
                switch (chooser.effectTargetType)
                {
                    case Enum_EffectTargetType.enemy:
                        List_Return = UtilsTargetChooser.FindLeastHP(character, enemys,  chooser.targetNumber, chooser.isTargetAll, chooser.isRangeInfinity, chooser.range);
                        break;

                    case Enum_EffectTargetType.ally:
                        List_Return = UtilsTargetChooser.FindLeastHP(character, allies, chooser.targetNumber, chooser.isTargetAll, chooser.isRangeInfinity, chooser.range);
                        break;
                }
                break;
            case Enum_EffectTargetChooser.mostHP:
                switch (chooser.effectTargetType)
                {
                    case Enum_EffectTargetType.enemy:
                        List_Return = UtilsTargetChooser.FindMostHP(character, enemys, chooser.targetNumber, chooser.isTargetAll, chooser.isRangeInfinity, chooser.range);
                        break;
                    case Enum_EffectTargetType.ally:
                        List_Return = UtilsTargetChooser.FindMostHP(character, allies, chooser.targetNumber, chooser.isTargetAll, chooser.isRangeInfinity, chooser.range);
                        break;
                }
                break;
            case Enum_EffectTargetChooser.ranged:
                switch (chooser.effectTargetType)
                {
                    case Enum_EffectTargetType.enemy:
                        List_Return = UtilsTargetChooser.FindInRange(character, enemys, chooser.range);
                        break;
                    case Enum_EffectTargetType.ally:
                        List_Return = UtilsTargetChooser.FindInRange(character, allies, chooser.range);
                        break;
                }
                break;
            
        }
        if (List_Return == null)
        {
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + "没有符合条件的目标，返回null");
        }
        return List_Return;
    }

    /// <summary>
    /// 返回半径内的所有ICharacter
    /// </summary>
    /// <param name="targets"></param>
    /// <returns></returns>
    public static List<Unit> FindInRange(Unit character, List<Unit> targets, float radius){
        Vector3 charPos = character.GetPosition();
        List<Unit> List_Return = new List<Unit>();
        targets.ForEach((target)=>{
            if(Vector3.Distance(charPos, target.GetPosition()) < radius){
                List_Return.Add(target);
            }
        });
        return List_Return;
    }
    /// <summary>
    /// 找出最近的一个目标
    /// </summary>
    public static Unit FindClosest(Unit character, List<Unit> targets){
        Vector3 charPos = character.GetPosition();
        Unit closestTarget = null;
        float MinDist = Unit.CONST_DETECT_RANGE;
        foreach(Unit target in targets){
            //已经阵亡的不计算
            if(target.IsKilled()){
                continue;
            }
            float dist = Vector3.Distance(charPos, target.GetPosition());
            if(dist < MinDist){
                MinDist = dist;
                closestTarget = target;
            }
        }
        return closestTarget;
    }
    /// <summary>
    /// 找出最近的n个目标, 以从最近到最远的顺序返回,只返回List<ICharacter>
    /// </summary>
    /// <param name="character"></param>
    /// <param name="targets"></param>
    /// <param name="number">返回目标的个数n</param>
    /// <param name="isTargetAll">可选参数，为true时则选择全体目标</param>
    /// <param name="radius">限制搜索的范围,大于范围的将不会被返回</param>
    /// <returns></returns>
    public static List<Unit> FindClosest(Unit character, List<Unit> targets, int number=1, bool isTargetAll = false, bool isRangeInfinity = false,float radius = 0f){
        List<Unit> List_Return = new List<Unit>();
        if(number<=0){
            Debug.LogWarning("FindClosest().number=0");
            return null;
        }
        if(number==1){
            List_Return.Add(FindClosest(character,targets));
            return List_Return;
        }
        if(isTargetAll){
            return FindClosest(character, targets, targets.Count);
        }
        if(radius<0f){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"radius不能为负数");
            return null;
        }

        Vector3 charPos = character.GetPosition();
        targets.Sort(
            (a,b)=>{
            if(a.IsKilled()){targets.Remove(a);} //阵亡的不计算
            if(b.IsKilled()){targets.Remove(b);}
            if(!isRangeInfinity){
                if(Vector3.Distance(charPos, a.GetPosition())>radius){targets.Remove(a);} //距离超过radius的不计算
                if(Vector3.Distance(charPos, b.GetPosition())>radius){targets.Remove(b);}
            }
            return Vector3.Distance(charPos, a.GetPosition()).CompareTo(Vector3.Distance(charPos, b.GetPosition()));
            }
        ); 
        
        return targets.GetRange(0, number); //获取前n个,数量不够则返回已经有的
    }

    /// <summary>
    /// 找出最远的目标
    /// </summary>
    public static Unit FindFarest(Unit character, List<Unit> targets){
        Vector3 charPos = character.GetPosition();
        Unit farestTarget = null;
        float MaxDist = 999f;
        foreach(Unit target in targets){
            //已经阵亡的不计算
            if(target.IsKilled()){
                continue;
            }
            float dist = Vector3.Distance(charPos, target.GetPosition());
            if(dist > MaxDist){
                MaxDist = dist;
                farestTarget = target;
            }
        }
        return farestTarget;
    }

    /// <summary>
    /// 找出最远的n个目标, 以从最远到最近的顺序返回，只返回List<ICharacter>
    /// </summary>
    /// <param name="character"></param>
    /// <param name="targets"></param>
    /// <param name="number"></param>
    /// <param name="isTargetAll">可选参数，为true时则选择全体目标</param>
    /// <param name="radius">限制搜索的范围,大于范围的将不会被返回</param>
    /// <returns></returns>
    public static List<Unit> FindFarest(Unit character, List<Unit> targets, int number = 1, bool isTargetAll = false, bool isRangeInfinity=false, float radius = 0f){
        List<Unit> List_Return = new List<Unit>();
        if(number<=0){
            Debug.LogWarning("FindFarest().number=0");
            return null;
        }
        if(number==1){
            List_Return.Add(FindFarest(character,targets));
            return List_Return;
        }
        if(isTargetAll){
            return FindFarest(character, targets, targets.Count);
        }
        if(radius<0f){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"radius不能为负数");
            return null;
        }
        Vector3 charPos = character.GetPosition();
        targets.Sort(
            (a,b)=>{
            if(a.IsKilled()){targets.Remove(a);} //阵亡的不计算
            if(b.IsKilled()){targets.Remove(b);}
            if(!isRangeInfinity){
                if(Vector3.Distance(charPos, a.GetPosition())>radius){targets.Remove(a);} //距离超过radius的不计算
                if(Vector3.Distance(charPos, b.GetPosition())>radius){targets.Remove(b);}
            }
            return -Vector3.Distance(charPos, a.GetPosition()).CompareTo(Vector3.Distance(charPos, b.GetPosition()));
            }
        ); 
        
        return targets.GetRange(0, number); //获取前n个,数量不够则返回已经有的
    }

    /// <summary>
    /// 找出血量最少的目标
    /// </summary>
    /// <param name="targets"></param>
    /// <returns></returns>
    public static Unit FindLeastHP(List<Unit> targets){
        Unit leastHPTarget = null;
        int MinHP = 0;
        foreach(Unit target in targets){
            //已经阵亡的不计算
            if(target.IsKilled()){
                continue;
            }
            int hp = target.Attribute.HP;
            if(hp < MinHP){
                MinHP = hp;
                leastHPTarget = target;
            }
        }
        return leastHPTarget;
    }

    /// <summary>
    /// 找出血量最少的n个目标, 从血量最少到最多的顺序返回, List<ICharacter>
    /// </summary>
    public static List<Unit> FindLeastHP(List<Unit> targets, int number = 1, bool isTargetAll = false){
        List<Unit> List_Return = new List<Unit>();
        if(number<=0){
            Debug.LogWarning("FindLeastHP().number=0");
            return null;
        }
        if(number==1){
            List_Return.Add(FindLeastHP(targets));
            return List_Return;
        }
        if(isTargetAll){
            return FindLeastHP(targets, targets.Count);
        }
        
        targets.Sort(
            (a,b)=>{
            if(a.IsKilled()){targets.Remove(a);} //阵亡的不计算
            if(b.IsKilled()){targets.Remove(b);}
            return a.Attribute.HP.CompareTo(b.Attribute.HP);
            }
        ); 
        
        return targets.GetRange(0, number); //获取前n个,数量不够则返回已经有的
    }

    /// <summary>
    /// 找出血量最少的n个目标, 从血量最少到最多的顺序返回, List<ICharacter>
    /// </summary>
    public static List<Unit> FindLeastHP(Unit character, List<Unit> targets, int number = 1, bool isTargetAll = false, bool isRangeInfinity=false, float radius = 0f){
        List<Unit> List_Return = new List<Unit>();
        if(number<=0){
            Debug.LogWarning("FindLeastHP().number=0");
            return null;
        }
        if(number==1){
            List_Return.Add(FindLeastHP(targets));
            return List_Return;
        }
        if(isTargetAll){
            return FindLeastHP(targets, targets.Count);
        }
        if(radius<0f){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"radius不能为负数");
            return null;
        }
        targets.Sort(
            (a,b)=>{
            if(a.IsKilled()){targets.Remove(a);} //阵亡的不计算
            if(b.IsKilled()){targets.Remove(b);}
            if(!isRangeInfinity){
                Vector3 charPos = character.GetPosition();
                if(Vector3.Distance(charPos, a.GetPosition())>radius){targets.Remove(a);} //距离超过radius的不计算
                if(Vector3.Distance(charPos, b.GetPosition())>radius){targets.Remove(b);}
            }
            return a.Attribute.HP.CompareTo(b.Attribute.HP);
            }
        ); 
        
        return targets.GetRange(0, number); //获取前n个,数量不够则返回已经有的
    }

    /// <summary>
    /// 找出血量最多的目标
    /// </summary>
    public static Unit FindMostHP(List<Unit> targets){
        Unit mostHPTarget = null;
        int MaxHP = Int32.MaxValue;
        foreach(Unit target in targets){
            //已经阵亡的不计算
            if(target.IsKilled()){
                continue;
            }
            int hp = target.Attribute.HP;
            if(hp > MaxHP){
                MaxHP = hp;
                mostHPTarget = target;
            }
        }
        return mostHPTarget;
    }

    /// <summary>
    /// 找出血量最多的目标，从血量最多到最少排序, List<ICharacter>
    /// </summary>
    public static List<Unit> FindMostHP(List<Unit> targets, int number = 1, bool isTargetAll = false){
         List<Unit> List_Return = new List<Unit>();
        if(number<=0){
            Debug.LogWarning("FindMostHP().number=0");
            return null;
        }
        if(number==1){
            List_Return.Add(FindMostHP(targets));
            return List_Return;
        }
        if(isTargetAll){
            return FindMostHP(targets, targets.Count);
        }

        targets.Sort(
            (a,b)=>{
            if(a.IsKilled()){targets.Remove(a);} //阵亡的不计算
            if(b.IsKilled()){targets.Remove(b);}
            return -a.Attribute.HP.CompareTo(b.Attribute.HP);
            }
        ); 
        
        return targets.GetRange(0, number); //获取前n个,数量不够则返回已经有的
    }

    /// <summary>
    /// 找出血量最多的目标，从血量最多到最少排序, List<ICharacter>,判断距离
    /// </summary>
    public static List<Unit> FindMostHP(Unit character, List<Unit> targets, int number = 1, bool isTargetAll = false, bool isRangeInfinity=false, float radius = 0f){
         List<Unit> List_Return = new List<Unit>();
        if(number<=0){
            Debug.LogWarning("FindMostHP().number=0");
            return null;
        }
        if(number==1){
            List_Return.Add(FindMostHP(targets));
            return List_Return;
        }
        if(isTargetAll){
            return FindMostHP(targets, targets.Count);
        }
        if(radius<0f){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"radius不能为负数");
            return null;
        }
        targets.Sort(
            (a,b)=>{
            if(a.IsKilled()){targets.Remove(a);} //阵亡的不计算
            if(b.IsKilled()){targets.Remove(b);}
            if(!isRangeInfinity){
                Vector3 charPos = character.GetPosition();
                if(Vector3.Distance(charPos, a.GetPosition())>radius){targets.Remove(a);} //距离超过radius的不计算
                if(Vector3.Distance(charPos, b.GetPosition())>radius){targets.Remove(b);}
            }
            return -a.Attribute.HP.CompareTo(b.Attribute.HP);
            }
        ); 
        
        return targets.GetRange(0, number); //获取前n个,数量不够则返回已经有的
    }


}

