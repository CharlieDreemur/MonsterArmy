using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 所有的有prefab的data必须继承它
/// </summary>
public class IDataWithPrefab: IData{
    public virtual UnityEngine.GameObject Prefab{get;set;}//预制体
}

/// <summary>
/// 所有的data必须继承它
/// </summary>
public abstract class IData:ScriptableObject{
}