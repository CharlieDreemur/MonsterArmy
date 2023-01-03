using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[FoldoutGroup("基本信息")]
[System.Serializable]
[HideLabel]
/// <summary>
/// struct是值类型所以不需要复制数据，赋值时不是引用
/// </summary>
public struct DataBaseInfo
{
    public string ID;
    public string name;
    [MultilineAttribute]
    public string description;

}
