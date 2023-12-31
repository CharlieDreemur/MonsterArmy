using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using MonsterArmy.Core.UnitSystem;
using MonsterArmy.Core.UnitSystem.Data;
//using Newtonsoft.Json;

//管理所有的ICharacter，负责ICharacter的生成和更新,ICharacter的对象池，之后会考虑将这两个功能分离并增加characterFactory
public class UnitManager: Singleton<UnitManager>, IManager
{   
    public SetupData setupData;
    [Header("Set in the Inspector")]
    [Header("Set Dynamically")]
    private UnityAction<string> initiateAllyAction;
    private UnityAction<string> initiateEnemyAction;
    public List<GameObject> List_AllyPrefabs = new List<GameObject>();
    public List<GameObject> List_EnemyPrefabs = new List<GameObject>();
    public Dictionary<int, Unit> Dic_ICharController = new Dictionary<int, Unit>();
    public List<Unit> List_Ally = new List<Unit>(); //玩家方所有单位的List
    public List<Unit> List_Enemy = new List<Unit>(); //敌人方所有单位的List

    public Transform parent;
    protected override void OnAwake()
    {
        initiateAllyAction = new UnityAction<string>(InitiateAlly);
        initiateEnemyAction = new UnityAction<string>(InitiateEnemy);
    }
    public void Init()
    {   
        EventManager.StartListening("InstantiateAlly", initiateAllyAction);
        EventManager.StartListening("InstantiateEnemy", initiateEnemyAction);
        for(int i = 0; i<List_AllyPrefabs.Count;i++){
            //EventManager.TriggerEvent("InstantiateAlly", JsonUtility.ToJson(List_AllyPrefab[i]));
            InitiateAlly(List_AllyPrefabs[i]);
        }

        for(int i = 0; i<List_EnemyPrefabs.Count;i++){
            //EventManager.TriggerEvent("InstantiateEnemy", JsonUtility.ToJson(List_EnemyPrefab[i]));
            InitiateEnemy(List_EnemyPrefabs[i]);
        }
        SetDictionaryToList(Dic_ICharController);
    }

    private void OnDisable() {
        EventManager.StopListening("InstantiateAlly", initiateAllyAction);
        EventManager.StopListening("InstantiateEnemy", initiateEnemyAction);
    }

    // 我们希望可以在MainManager手动控制Update顺序
    public void OnUpdate()
    {

        //遍历字典的ICharacter并更新
        foreach(Unit item in Dic_ICharController.Values)
        {
            if(item.isKilled){
                if(List_Enemy.Contains(item)){
                    List_Enemy.Remove(item);
                }
                if(List_Ally.Contains(item)){
                    List_Ally.Remove(item);
                }
                continue;
            }
            //更新目标
            switch (item.EntityType){
                case UnitType.Ally:
                    item.GetCharacterAbility().UpdateTargets(List_Enemy, List_Ally);
                    break;
                case UnitType.Enemy:
                    item.GetCharacterAbility().UpdateTargets(List_Ally, List_Enemy);
                    break;
            }
            //item.Update();
        }

    }


    //将Dic_ICharController按照阵营添加进不同的List
    public void SetDictionaryToList(Dictionary<int, Unit> _Dic_ICharController){
        List_Ally.Clear();
        List_Enemy.Clear();
        foreach(Unit item in _Dic_ICharController.Values){
            switch (item.EntityType){
                case UnitType.Ally:
                    List_Ally.Add(item);
                    break;
                case UnitType.Enemy:
                    List_Enemy.Add(item);
                    break;
            }
         
        }
    }
    public void InitiateAlly(string jsonValue){
        GameObject args =  JsonUtility.FromJson<GameObject>(jsonValue);
        InitiateAlly(args);
    }

    public void InitiateEnemy(string jsonValue){
        GameObject args =  JsonUtility.FromJson<GameObject>(jsonValue);
        InitiateEnemy(args);
    }

    public void InitiateAlly(GameObject prefab){
        if(prefab==null){
            Debug.LogWarning("InitiateAlly: Null prefab");
            return;
        }
        GameObject obj = InitiateEntity(prefab);
        if (obj == null) return;
        Unit unitController = obj.AddComponent<Unit>();
        if(parent != null){
            obj.transform.SetParent(parent);
        }

        //传入初始敌方友军列表
        unitController.SetCharacterAbility(new AbilityController(unitController, List_Enemy, List_Ally));
        //注意一定要确保所有模块都添加好了再Init()
        unitController.Init();
        Dic_ICharController.Add(obj.GetInstanceID(), unitController);
        
    }


     public void InitiateEnemy(GameObject prefab){
        if(prefab==null){
            Debug.LogWarning("InitiateEnemy: Null prefab");
             return;
        }
        GameObject obj = InitiateEntity(prefab);
        if (obj == null) return;
        Unit unitController = obj.AddComponent<Unit>();
        //传入初始敌方友军列表
        unitController.SetCharacterAbility(new AbilityController(unitController, List_Enemy, List_Ally));  
        //注意一定要确保所有模块都添加好了再Init()      
        unitController.Init();
        Dic_ICharController.Add(obj.GetInstanceID(), unitController);
        
    }


    //上面两个的公用部分
    public GameObject InitiateEntity(GameObject prefab){
        if(prefab == null){
            Debug.LogWarning(this.name+"没有对应index的prefabs");
            return null;
        }
        GameObject obj = Instantiate(prefab);
        if(parent != null){
            obj.transform.SetParent(parent);
        }
        UnitComponentsFactoryUtils.AddAllComponents(setupData, obj);
        return obj;
    }
    /*
    @function 删除一个索引为指定ID/Key的元素
    */
    public void RemoveEntityWithID(int ID){
        if(!Dic_ICharController.ContainsKey(ID)){
            Debug.LogWarning(this.name+"Dic_ICharController没有key为该ID的元素");
            return;
        }
        Dic_ICharController.Remove(ID);
    }

      /*
    @function 删除一个索引为指定ID/Key的元素
    */
    public void RemoveEntityWithValue(Unit IChar){
        if(!Dic_ICharController.ContainsValue(IChar)){
            Debug.LogWarning(this.name+"Dic_ICharController没有value为该IChar的元素");
            return;
        }
        
    }

    /*
    @function 在字典中返回为对应值的索引
    */
    public int GetID(Unit IChar){
        foreach(int key in Dic_ICharController.Keys){
            if(Dic_ICharController[key] == IChar){
                return key;
            }
        }
        Debug.LogWarning("Dic_ICharController未找到值为"+IChar+"的索引，默认返回-1");
        return -1;
    }


    /*
    @function 获取对应ID的Icharacter
    */
    public Unit GetEntity(int ID){
        return Dic_ICharController[ID];
    }

    /*
    @function 获取对应obj的Icharacter
    */
    public Unit GetEntity(GameObject obj){
        return GetEntity(obj.GetInstanceID());
    }
    
}
