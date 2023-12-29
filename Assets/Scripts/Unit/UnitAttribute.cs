using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using YamlDotNet.Serialization;
[System.Serializable]
[HideLabel]
public class UnitAttribute
{
    [MinValue(0)]
    [GUIColor(0f, 1f, 0.6f, 1f)]
    [LabelWidth(70f)]
    [LabelText("HP")]
    [SerializeField]
    private int hp;


    [MinValue(0)]
    [GUIColor(0f, 0.6f, 1f, 1f)]
    [LabelWidth(70f)]
    [LabelText("MP")]
    [SerializeField]
    private int mp;


    public int HP
    {
        set
        {
            hp = Mathf.Clamp(value, 0, maxhp.FinalAttribute);
        }
        get
        {
            return hp;
        }
    }
    public int MP
    {
        set
        {
            mp = Mathf.Clamp(value, 0, maxmp.FinalAttribute);
        }
        get
        {
            return mp;
        }
    }
    [Title("Basic Property")]
    [GUIColor(0f, 1f, 0f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    public IntNumeric maxhp;

    [GUIColor(0.2f, 0.5f, 1f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    public IntNumeric maxmp;


    [GUIColor(1f, 0f, 0f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    public IntNumeric atk;
    [GUIColor(0.9f, 0.6f, 0f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    public IntNumeric def;
    [GUIColor(0.7f, 0.3f, 0.9f, 1f)]
    [InlineProperty]
    [LabelWidth(70f)]
    public IntNumeric ap;

    [Title("Special Property")]

    [SerializeField]
    [MinValue(-0.9f)]
    [MaxValue(9f)]
    [LabelWidth(70f)]
    //[LabelText("攻击速度")]
    private FloatNumeric atkspd; //攻击速度加成

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(99f)]
    [LabelWidth(70f)]
    //[LabelText("移动速度")]
    private FloatNumeric movespd; //移动速度

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(999f)]
    [LabelWidth(70f)]
    //[LabelText("攻击范围")]
    private FloatNumeric atkRange; //攻击范围

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(1f)]
    [LabelWidth(70f)]
    //[LabelText("暴击几率")]
    private FloatNumeric critChance; //暴击几率

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(999f)]
    [LabelWidth(70f)]
    //[LabelText("暴击伤害")]
    private FloatNumeric critDamage; //暴击伤害，暴击默认造成2倍伤害

    [SerializeField]
    [MinValue(0f)]
    [MaxValue(1f)]
    [LabelWidth(70f)]
    //[LabelText("闪避几率")]
    private FloatNumeric dodgeChance; //闪避几率

    public TextAsset yamlFile;
    public int importId;

    public float DamageReduction
    {
        get
        {
            return (0.9f * def.FinalAttribute) / (20f + def.FinalAttribute);
        }
    }
    public float ATKSPD
    {
        get
        {
            return atkspd.FinalAttribute;
        }
    }

    [ShowInInspector]
    public float AtkInterval
    {
        get
        {
            if (ATKSPD > -1f)
            {
                return 2 / (1 + ATKSPD);
            }
            else
            {
                return 0;
            }

        }
    }

    public float MoveSpeed
    {
        get
        {
            return movespd.FinalAttribute;
        }
    }

    public float AtkRange
    {
        get
        {
            return atkRange.FinalAttribute;
        }
    }

    public float CritChance
    {
        get
        {
            return critChance.FinalAttribute;
        }
    }

    public float CritDamage
    {
        get
        {
            return critDamage.FinalAttribute;
        }
    }

    public float DodgeChance
    {
        get
        {
            return dodgeChance.FinalAttribute;
        }
    }


    /// <summary>
    /// 按照data Init()所有数据，包括NowHP或NowMP,并根据情况回复所有生命值和法力值
    /// </summary>
    /// <param name="config"></param>
    public void Init(UnitAttributeConfig config)
    {
        maxhp.BaseAttribute = config.MAXHP;
        maxmp.BaseAttribute = config.MAXMP;
        atk.BaseAttribute = config.ATK;
        def.BaseAttribute = config.DEF;
        ap.BaseAttribute = config.AP;
        atkspd.BaseAttribute = config.ATKSPD;
        movespd.BaseAttribute = config.MOVSPD;
        atkRange.BaseAttribute = config.ATKRange;
        critChance.BaseAttribute = config.CritChance;
        critDamage.BaseAttribute = config.CritDamage;
        dodgeChance.BaseAttribute = config.DodgeChance;
    }


    [Button("Reset", ButtonSizes.Medium)]
    public void Reset()
    {
        maxhp.Reset();
        maxmp.Reset();
        atk.Reset();
        def.Reset();
        ap.Reset();
        atkspd.Reset();
        movespd.Reset();
        movespd.BaseAttribute = 2f;
        atkRange.Reset();
        atkRange.BaseAttribute = 5f;
        critChance.Reset();
        critDamage.Reset();
        critDamage.BaseAttribute = 1.5f;
        dodgeChance.Reset();
    }

    [Button("Clear", ButtonSizes.Medium)]
    public void Clear()
    {
        maxhp.Reset();
        maxmp.Reset();
        atk.Reset();
        def.Reset();
        ap.Reset();
        atkspd.Reset();
        movespd.Reset();
        atkRange.Reset();
        critChance.Reset();
        critDamage.Reset();
        dodgeChance.Reset();
    }

    [ContextMenu("Load Enemy Data")]
    private void LoadEnemyData()
    {
        if (yamlFile == null)
        {
            Debug.LogError("YAML file is not assigned!");
            return;
        }

        var deserializer = new DeserializerBuilder().Build();
        Init(deserializer.Deserialize<UnitAttributeConfig>(yamlFile.text));

        // Use enemyData to set up the enemy's properties
    }

}


