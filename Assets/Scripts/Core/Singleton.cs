using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Every Singleton please inherits from it, if you don't want serialized property supportted by odin, changed the SerializedMonoBehaviour to MonoBehaviour
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : SerializedMonoBehaviour where T:Singleton<T>
{
    //单例
    public static T Instance {get; private set;}
    protected virtual void Awake(){
        if(Instance == null){
            Instance = (T) this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Debug.LogWarning("Instance Already Exist");
            Destroy(this.gameObject);
        }
    }
}
