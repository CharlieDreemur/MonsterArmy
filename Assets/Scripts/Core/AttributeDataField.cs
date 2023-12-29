using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;


/// <summary>
/// ICharacter请使用这个数据Struct
/// </summary>

[System.Serializable]
[HideLabel]
public struct AttributeDataField
{
    public AttributeDataChanged nowData;
    public AttributeDataFixed fixedData;
    public int NowHP
    {
        set
        {
            nowData.nowHP = Mathf.Clamp(value, 0, fixedData.maxHP.FinalAttribute);
        }
        get
        {
            return nowData.nowHP;
        }
    }
    public int NowMP
    {
        set
        {
            nowData.nowMP = Mathf.Clamp(value, 0, fixedData.maxMP.FinalAttribute);
        }
        get
        {
            return nowData.nowMP;
        }
    }

    public void Init(AttributeDataField data)
    {
        fixedData.Init(data.fixedData);
        InitNowData(data);
    }

    public void InitNowData(AttributeDataField data){
        nowData.isAutoNowHP = data.nowData.isAutoNowHP;
        nowData.isAutoNowMP = data.nowData.isAutoNowMP;
        InitHPAndMP(data);
    }
    public void InitHPAndMP(AttributeDataField data)
    {
        InitHPAndMP(data.nowData.nowHP, data.nowData.nowMP);
    }

    public void InitHPAndMP(int nowHP, int nowMP)
    {
        NowHP = nowHP;
        NowMP = nowMP;
        InitHPAndMP();
    }

    public void InitHPAndMP()
    {
        if (nowData.isAutoNowHP)
        {
            FillHP();
        }
        if (nowData.isAutoNowMP)
        {
            FillMP();
        }
    }
    public void FillHP()
    {
        NowHP = fixedData.maxHP.FinalAttribute;
    }

    public void FillMP()
    {
        NowMP = fixedData.maxMP.FinalAttribute;
    }

}


[FoldoutGroup("属性数据")]
[Title("NowProperty")]
[HideLabel]
[System.Serializable]
public struct AttributeDataChanged
{
    [MinValue(0), GUIColor(0f, 1f, 0.6f, 1f), HorizontalGroup("NowHP"), DisableIf("isAutoNowHP")]
    [LabelWidth(70f)]
    [LabelText("HP")]
    public int nowHP;

    
    [HorizontalGroup("NowHP"), HideLabel]
    public bool isAutoNowHP;

    [MinValue(0)]
    [GUIColor(0f, 0.6f, 1f, 1f)]
    [HorizontalGroup("NowMP")]
    [DisableIf("isAutoNowMP")]
    [LabelWidth(70f)]
    [LabelText("MP")]
    public int nowMP; //当前法力值

    [HorizontalGroup("NowMP"), HideLabel]
    public bool isAutoNowMP;

}



[FoldoutGroup("属性数据")]
[HideLabel]
[System.Serializable]
public struct AttributeDataFixed
{
    
    [Title("Basic Property")]
    [GUIColor(0f, 1f, 0f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    //[LabelText("最大生命值")]
    public IntNumeric maxHP;

    [GUIColor(0.2f, 0.5f, 1f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    //[LabelText("最大法力值")]
    public IntNumeric maxMP;


    [GUIColor(1f, 0f, 0f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    //[LabelText("攻击力")]
    public IntNumeric atk;
    [GUIColor(0.9f, 0.6f, 0f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    //[LabelText("防御力")]
    public IntNumeric def;
    [GUIColor(0.7f, 0.3f, 0.9f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    //[LabelText("法强")]
    public IntNumeric ap;

    [Title("Special Property")]
    [SerializeField]
    [MinValue(0)]
    [LabelWidth(70f)]
    //[LabelText("护盾值")]
    private int shieldValue; //护盾值

    [SerializeField]
    [MinValue(-0.9f)]
    [MaxValue(9f)]
    [LabelWidth(70f)]
    //[LabelText("攻击速度")]
    private float atkSpeed; //攻击速度加成

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(99f)]
    [LabelWidth(70f)]
    //[LabelText("移动速度")]
    private float moveSpeed; //移动速度

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(999f)]
    [LabelWidth(70f)]
    //[LabelText("攻击范围")]
    private float atkRange; //攻击范围

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(1f)]
    [LabelWidth(70f)]
    //[LabelText("暴击几率")]
    private float critChance; //暴击几率

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(999f)]
    [LabelWidth(70f)]
    //[LabelText("暴击伤害")]
    private float critDamage; //暴击伤害，暴击默认造成2倍伤害

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(1f)]
    [LabelWidth(70f)]
    //[LabelText("闪避几率")]
    private float dodgeChance; //闪避几率



    public int ShieldValue
    {
        set
        {
            if (value > 0)
            {
                shieldValue = value;
            }
            else
            {
                shieldValue = 0;
            }
        }
        get
        {
            return shieldValue;
        }
    }



    public float DamageReduction
    {
        get
        {
            return (0.9f * def.FinalAttribute) / (20f + def.FinalAttribute);
        }
    }
    public float AtkSpeed
    {
        set
        {
            atkSpeed = Mathf.Clamp(value, -0.9f, 9f); //最慢攻速-90%（10s1下），最快攻速900% （1s10下）
        }
        get
        {
            return atkSpeed;
        }
    }

    [ShowInInspector]
    //攻击间隔，只读，要修改请修改攻击速度
    public float AtkInterval
    {
        get
        {
            if (AtkSpeed > -1f)
            {
                return 2 / (1 + AtkSpeed);
            }
            else
            {
                return 0;
            }


        }
    }

    public float MoveSpeed
    {
        set
        {
            moveSpeed = Mathf.Clamp(value, 0f, 99f);
        }
        get
        {
            return moveSpeed;
        }
    }

    public float AtkRange
    {
        set
        {
            atkRange = Mathf.Clamp(value, 0f, 999f);
        }
        get
        {
            return atkRange;
        }
    }

    public float CritChance
    {
        set
        {
            critChance = Mathf.Clamp(value, 0f, 1f); //暴击几率0-100%
        }
        get
        {
            return critChance;
        }
    }

    public float CritDamage
    {
        set
        {
            critDamage = Mathf.Clamp(value, 0f, 999f); //暴击伤害，最高999倍
        }
        get
        {
            return critDamage;
        }
    }

    public float DodgeChance
    {
        set
        {
            dodgeChance = Mathf.Clamp(value, 0f, 1f);
        }
        get
        {
            return dodgeChance;
        }
    }


    /// <summary>
    /// 按照data Init()所有数据，包括NowHP或NowMP,并根据情况回复所有生命值和法力值
    /// </summary>
    /// <param name="data"></param>
    public void Init(AttributeDataFixed data)
    {
        maxHP.Init(data.maxHP);
        maxMP.Init(data.maxMP);
        atk.Init(data.atk);
        def.Init(data.def);
        ap.Init(data.ap);
        ShieldValue = (data.ShieldValue);
        AtkSpeed = data.AtkSpeed;
        MoveSpeed = data.MoveSpeed; ;
        AtkRange = data.AtkRange;
        CritChance = data.CritChance;
        CritDamage = data.CritDamage;
        DodgeChance = data.DodgeChance;
    }

    public void Add(AttributeDataFixed data){
        maxHP.Add(data.maxHP);
        maxMP.Add(data.maxMP);
        atk.Add(data.atk);
        def.Add(data.def);
        ap.Add(data.ap);
        ShieldValue += (data.ShieldValue);
        AtkSpeed += data.AtkSpeed;
        MoveSpeed += data.MoveSpeed;
        AtkRange += data.AtkRange;
        CritChance += data.CritChance;
        CritDamage += data.CritDamage;
        DodgeChance += data.DodgeChance;
    }
    /// <summary>
    /// 不影响NowHP和NowMP
    /// </summary>
    public void Minus(AttributeDataFixed data){
        maxHP.Minus(data.maxHP);
        maxMP.Minus(data.maxMP);
        atk.Minus(data.atk);
        def.Minus(data.def);
        ap.Minus(data.ap);
        ShieldValue += (data.ShieldValue);
        AtkSpeed -= data.AtkSpeed;
        MoveSpeed -= data.MoveSpeed;
        AtkRange -= data.AtkRange;
        CritChance -= data.CritChance;
        CritDamage -= data.CritDamage;
        DodgeChance -= data.DodgeChance;
    }



    [Button("Reset", ButtonSizes.Medium)]
    public void Reset()
    {
        maxHP.Reset();
        maxMP.Reset();
        atk.Reset();
        def.Reset();
        ap.Reset();
        ShieldValue = 0;
        AtkSpeed = 0f;
        MoveSpeed = 2f;
        AtkRange = 5f;
        CritChance = 0f;
        CritDamage = 2f;
        DodgeChance = 0f;
    }

    [Button("Clear", ButtonSizes.Medium)]
    public void Clear()
    {
        maxHP.Reset();
        maxMP.Reset();
        atk.Reset();
        def.Reset();
        ap.Reset();
        ShieldValue = 0;
        AtkSpeed = 0f;
        MoveSpeed = 0f;
        AtkRange = 0f;
        CritChance = 0f;
        CritDamage = 0f;
        DodgeChance = 0f;
    }
}

