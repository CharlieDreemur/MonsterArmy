using UnityEngine;


/// <summary>
/// 对象池的物体必须继承这个接口
/// </summary>
public interface IPoolObject{
    /// <summary>
    /// 当对象从对象池创建时
    /// </summary>
    void OnSpawn();

    /// <summary>
    /// 当对象被（销毁）移回对象池时
    /// </summary>
    void OnRelease();
}
