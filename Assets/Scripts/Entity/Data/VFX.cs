using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



//特效释放位置
[EnumToggleButtons] 
public enum Enum_VFXLocation{
    self, //释放在自己身上(角色中心)

    selfWithFollow, //释放在自己身上且跟随直到消失
    
    target, //释放在目标身上,如果有多个目标，每个目标都会有这个特效(目标中心)

    targetWithFollow, //释放在目标身上且跟随直到消失,如果有多个目标，每个目标都会有这个特效(目标中心)
    
}


[FoldoutGroup("特效设置")] [System.Serializable]
public class VFX
{ 
    //游戏特效Unit
    //[LabelText("游戏特效模型")]
    public UnityEngine.GameObject prefab; //游戏特效预制体

    public Enum_VFXLocation locationType;

    //[LabelText("特效坐标")]
    public Vector3 relativeposition = Vector3.zero; //以自己or目标中心为（0，0，0）进行偏移

    //[LabelText("放大倍数")]

    public Vector3 scale = Vector3.one; //特效的放大倍数

    //[LabelText("播放倍速")]

    public float speed; //特效的播放倍速

    public float startDelay; //延后多少s才播放该特效


    public UnityEngine.GameObject spawnVFX(){
        UnityEngine.GameObject spawnedVFX = UnityEngine.GameObject.Instantiate(prefab) as UnityEngine.GameObject;
        spawnedVFX.transform.localScale = scale;

        //TODO:使用回收池以优化
        UnityEngine.GameObject.Destroy(spawnedVFX, 5f);
        return spawnedVFX;
    }
  
}
