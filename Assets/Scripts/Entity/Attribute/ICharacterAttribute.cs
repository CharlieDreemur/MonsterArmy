using System.Data.Common;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
//Character的数据，用于计算和表示角色obj的属性（变量）
public class ICharacterAttribute : MonoBehaviour
{
[ShowInInspector] [Sirenix.OdinInspector.ReadOnly] [Tooltip("角色各自的数据和他们的data文件是解绑的，是复制的关系而不是引用的关系。")]
public ICharacterData data; 
#region PrivateProperty
    protected string characterName;
    protected Color color = Color.white;
    protected Vector3 scale;
    protected Material material;

    public AttributeDataField attributeData;
    public Vector3 Transform_SpawnPos = Vector3.zero; //ICharacter出生的坐标
    public ProjectileData projectileData;
    public List<AbilityData> List_AbilityData; //关于技能的List
    
#endregion

#region Property访问器
    public string CharacterName{
        set{
            characterName = value;
        }
        get{
            return characterName;
        }
    }

    public Color Color{
        set{
            color = value;
        }
        get{
            return color;
        }
    }

    public Vector3 Scale{
        set{
            scale = value;
        }
        get{
            return scale;
        }
    }

    public Material Material{
        set{
            material = value;
        }
        get{
            return material;
        }
    }
    
   
#endregion
    /// <summary>
    /// 按照data来初始化所有角色属性
    /// </summary>
    /// <param name="data"></param>
    public virtual void Init(ICharacterData data){
        this.data = data;
        characterName = data.name;
        Color = data.color;
        Scale = data.scale;
        Material = data.material;
        Transform_SpawnPos = data.Transform_SpawnPos;
        projectileData = data.projectileData;
        InitAttributeData(data.attributeData);
        //List_AbilityData =new List<AbilityData>(data.List_AbilityData); //List的复制，对原始值的修改不会影响此值
        List_AbilityData = data.List_AbilityData; //List的引用，修改原始值时此指也会修改
    }

    
    /// <summary>
    /// 初始化所有数据属性（AttributeData)
    /// </summary>
    /// <param name="data"></param>
    public void InitAttributeData(AttributeDataField data){
       attributeData.Init(data);
       
    }
   
    public bool IsCrit(){
        return (attributeData.fixedData.CritChance >= Random.Range(0f,1f));
    }

    public bool IsDodge(){
        return (attributeData.fixedData.DodgeChance >=Random.Range(0f,1f));
    }

    #region GetDamage方法组
        /*
    @funciton 返回敌人对我方的一次攻击造成的伤害，考虑闪避，暴击，防御等因素
    */
    public int GetDamage(Entity attacker, out bool IsDodge, out Enum_DamageType damageType){
        return GetDamage(attacker.GetCharacterAttribute(), this, out IsDodge, out damageType);
    }


    /*
    @funciton:给予两个人物，返回一方对另一方的一次攻击造成的伤害，考虑闪避，暴击，防御等因素
    */
    public static int GetDamage(Entity attacker, Entity target, out bool IsDodge, out Enum_DamageType damageType){
    
        return GetDamage(attacker.GetCharacterAttribute(), target.GetCharacterAttribute(), out IsDodge, out damageType);
    }

    /*
    @function: 给予两个角色属性，返回一方对另一方的一次攻击造成的伤害，考虑闪避，暴击，防御等因素
    */
    public static int GetDamage(ICharacterAttribute attacker_Attr, ICharacterAttribute target_Attr, out bool IsDodge, out Enum_DamageType damageType){
        IsDodge = false;
        damageType = Enum_DamageType.normal;
        
        int atkDamage = attacker_Attr.attributeData.fixedData.atk.FinalAttribute;
        if(target_Attr.IsDodge()){
            
            IsDodge = true;
            return 0;
        }

        if(attacker_Attr.IsCrit()){
            Debug.Log(attacker_Attr.CharacterName+"暴击了");
            atkDamage = (int)(atkDamage*attacker_Attr.attributeData.fixedData.CritDamage);
            damageType = Enum_DamageType.crit;
        }
    
        return (int)(atkDamage*(1-target_Attr.attributeData.fixedData.DamageReduction));;
    }
   
    #endregion


//先算伤害，再算防御
/// <summary>
/// 使该角色减少damage的血量,如果有护盾先减护盾
/// </summary>
/// <param name="damage"></param>
    public void UnderHurt(int damage){
        if(attributeData.fixedData.ShieldValue >0){
            if(attributeData.fixedData.ShieldValue >= damage){
                attributeData.fixedData.ShieldValue -= damage;
            }
            else{
                damage -= attributeData.fixedData.ShieldValue;
                attributeData.fixedData.ShieldValue = 0;
                attributeData.NowHP -= damage;
                
            }
        }
        else{
            attributeData.NowHP -= damage;
        }
    }

/*
@function 使该角色增加_healAmount的血量
*/

    public void UnderHeal(int healAmount){
        attributeData.NowHP += healAmount;
    }

}


