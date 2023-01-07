using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 所有的有prefab的data必须继承它
/// </summary>
public interface IPoolData{
    public UnityEngine.GameObject Prefab{get;set;} //预制体
}
