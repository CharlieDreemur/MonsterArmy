using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[EnumToggleButtons]
public enum Enum_ProjectileType{
    none,
    straight,
    parabola,
    track, //跟踪导弹
    

}


[CreateAssetMenu(fileName = "ProjectileData", menuName = "TheMonsterArmy/ScriptableObject/ProjectileData", order = 0)]

public class ProjectileData : IDataWithPrefab{
    public DataBaseInfo baseInfo;

    [FoldoutGroup("投掷物基本设置")] [SerializeField]
    private UnityEngine.GameObject prefab;
    public override UnityEngine.GameObject Prefab{
        get{
            return prefab;
        } 
        set{
            prefab = value;
        }
    }

    [FoldoutGroup("投掷物基本设置")]
    public float lifeCycle = 0f; //生命周期，生命周期一到自动回收

    [FoldoutGroup("投掷物基本设置")]
    public Color color = Color.white; //子弹的颜色


    [FoldoutGroup("投掷物基本设置")]
    public Vector3 scale = Vector3.one; //子弹的大小

    [FoldoutGroup("投掷物基本设置")]
    public Enum_ProjectileType projectileType = Enum_ProjectileType.none;//子弹类型

    [FoldoutGroup("投掷物细节设置")]

    [MinValue(0f)] [MaxValue(999f)] 
    public int velocity = 10; //子弹移动速度
    [FoldoutGroup("投掷物细节设置")] [ShowIf("projectileType", Enum_ProjectileType.parabola)]
    public bool isGreaterAngle = false;

    [SerializeField] [FoldoutGroup("投掷物修正")] 
    public float heightCorrection = 0f;

    /*
    [ShowIf("projectileType", Enum_ProjectileType.parabola)] [FoldoutGroup("投掷物细节设置")] [HorizontalGroup("投掷物细节设置/高度")]
    public float height = 15; //发射最高距离

    [ShowIf("projectileType", Enum_ProjectileType.parabola)] [HideLabel]  [FoldoutGroup("投掷物细节设置")] [HorizontalGroup("投掷物细节设置/高度")]
    public bool isAutoHeight = false; //自动高度
    
    [ShowIf("projectileType", Enum_ProjectileType.parabola)] [FoldoutGroup("投掷物细节设置")]
    public float parabolaDirection = 45; //发射角度
    */
    [ShowIf("projectileType", Enum_ProjectileType.parabola)] [FoldoutGroup("投掷物细节设置")]
    public int gravityScale = 1; //重力系数

    


    
   
    
}