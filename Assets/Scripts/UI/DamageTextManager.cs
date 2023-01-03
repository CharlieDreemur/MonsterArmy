using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class DamageTextManager : MonoBehaviour
{
    private UnityAction<string> action;
    public static DamageTextData data;
    private void Awake(){  
        data = Resources.Load("Data/UI/DamageTextData") as DamageTextData;
        action = new UnityAction<string>(Create);
    }
    private void OnEnable() {
        EventManager.StartListening("CreateDamageText", action);
    }
    private void OnDisable() {
        EventManager.StopListening("CreateDamageText", action);
    }
    
    public static void Create(string jsonValue){
        CreateDamageTextEventArgs args =  JsonUtility.FromJson<CreateDamageTextEventArgs>(jsonValue);
        Create(args);
    }

    public static void Create(CreateDamageTextEventArgs args){
        if(data==null){
            Debug.LogWarning("没有提供伤害跳字的数据");
            //return null;
        }
        //GameObject damageTextGameObject = Instantiate(data.prefab, pos, Quaternion.identity);
        UnityEngine.GameObject damageTextGameObject = ObjectPooler.Instance.Spawn(data, args.pos, Quaternion.identity);
        DamageText damageText = damageTextGameObject.GetComponent<DamageText>();
        damageText.Init(data, args.damageAmount, args.damageType);
        damageText.OnObjectSpawn();
        //return damageText;
    }
    
}

[System.Serializable]
public struct CreateDamageTextEventArgs{
    public CreateDamageTextEventArgs(Vector3 pos, int damageAmount, Enum_DamageType damageType)
    {
        this.pos = pos;
        this.damageAmount = damageAmount;
        this.damageType = damageType;
    }
    public Vector3 pos;
    public int damageAmount;
    public Enum_DamageType damageType;
}