using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//control all the componets that will be auto setup to each characters
public static class EntityComponetsControllerUtils
{


    public static void AddAllComponents(SetupData data, GameObject obj){
        //必须添加组件
        if(obj == null){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"gameobject does not exist");
            return;
        }
        AddEntityAnimation(obj);

        //可选添加组件
        if(data == null){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"setup data does not exist");
            return;
        }
        AddHealthBar(data, obj);
        AddRigidBody2D(data, obj);
        AddCapsuleCollider2D(data, obj);
        //Debug.Log(obj.name+"组件添加成功");
    }

    /// <summary>
    /// 必须添加组件，角色动画
    /// </summary>
    /// <param name="data"></param>
    /// <param name="character"></param>
    public static void AddEntityAnimation(GameObject character){
        //CheckComponents
        if(character.gameObject.GetComponent<AnimationController>()!=null){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"components already exist");
            return;
        }
        character.gameObject.AddComponent<AnimationController>();

    }


    public static void AddHealthBar(SetupData data, GameObject character){
        if(!data.isHealthBar){
            return;
        }
        //CheckComponents
        if(character.gameObject.GetComponent<HealthBar>()!=null){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"components already exist");
            return;
        }
        HealthBar toReturn = character.gameObject.AddComponent<HealthBar>();
        toReturn.data = data.healthBarData;

    }

    public static void AddRigidBody2D(SetupData data, GameObject character){
        if(!data.isRigidBody2D){
            return;
        }
        //CheckComponents
        if(character.gameObject.GetComponent<Rigidbody2D>()!=null){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"components already exist");
            return;
        }
        Rigidbody2D toReturn = character.gameObject.AddComponent<Rigidbody2D>();
        SetRigidBody2D(toReturn);

    }

     public static void AddCapsuleCollider2D(SetupData data, GameObject character){
        if(!data.isRigidBody2D){
            return;
        }
        //CheckComponents
        if(character.gameObject.GetComponent<CapsuleCollider2D>()!=null){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"components already exist");
            return;
        }
        CapsuleCollider2D toReturn = character.gameObject.AddComponent<CapsuleCollider2D>();
        SetCapsuleCollider2D(toReturn, data);

    }


    public static void SetRigidBody2D(Rigidbody2D obj){
        if(obj==null){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"obj does not exist");
            return;
        }
        obj.bodyType = RigidbodyType2D.Kinematic;
    }

    public static void SetCapsuleCollider2D(CapsuleCollider2D obj, SetupData data){
        if(obj==null){
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name+"obj does not exist");
            return;
        }
        obj.offset = data.CapsuleCollider2D_offset;
        obj.size = data.CapsuleCollider2D_size;
    }
}
