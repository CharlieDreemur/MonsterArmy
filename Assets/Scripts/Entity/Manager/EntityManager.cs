using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
//using Newtonsoft.Json;

//管理所有的ICharacter，负责ICharacter的生成和更新,ICharacter的对象池，之后会考虑将这两个功能分离并增加characterFactory
public class EntityManager: Singleton<EntityManager>, IManager
{   
    public SetupData setupData;
    [Header("Set in the Inspector")]
    [Header("Set Dynamically")]
    private UnityAction<string> initiateAllyAction;
    private UnityAction<string> initiateEnemyAction;
    public List<AllyData> List_AllyData = new List<AllyData>();
    public List<EnemyData> List_EnemyData = new List<EnemyData>();
    public Dictionary<int, Entity> Dic_ICharController = new Dictionary<int, Entity>();
    public List<Entity> List_Ally = new List<Entity>(); //玩家方所有单位的List
    public List<Entity> List_Enemy = new List<Entity>(); //敌人方所有单位的List

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
        for(int i = 0; i<List_AllyData.Count;i++){
            EventManager.TriggerEvent("InstantiateAlly", JsonUtility.ToJson(new InstantiateAllyArgs(List_AllyData[i])));
            //InitiateAlly(List_AllyData[i]);
        }

        for(int i = 0; i<List_EnemyData.Count;i++){
            EventManager.TriggerEvent("InstantiateEnemy", JsonUtility.ToJson(new InstantiateEnemyArgs(List_EnemyData[i])));
            //InitiateEnemy(List_EnemyData[i]);
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
        foreach(Entity item in Dic_ICharController.Values)
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
            switch (item.type_Character){
                case Enum_Character.Character:
                    item.GetCharacterAbility().UpdateTargets(List_Enemy, List_Ally);
                    break;
                case Enum_Character.Enemy:
                    item.GetCharacterAbility().UpdateTargets(List_Ally, List_Enemy);
                    break;
            }
            //item.Update();
        }

    }


    //将Dic_ICharController按照阵营添加进不同的List
    public void SetDictionaryToList(Dictionary<int, Entity> _Dic_ICharController){
        List_Ally.Clear();
        List_Enemy.Clear();
        foreach(Entity item in _Dic_ICharController.Values){
            switch (item.type_Character){
                case Enum_Character.Character:
                    List_Ally.Add(item as Ally);
                    break;
                case Enum_Character.Enemy:
                    List_Enemy.Add(item as Enemy);
                    break;
            }
         
        }
    }
    public void InitiateAlly(string jsonValue){
        InstantiateAllyArgs args =  JsonUtility.FromJson<InstantiateAllyArgs>(jsonValue);
        InitiateAlly(args.data);
    }

    public void InitiateEnemy(string jsonValue){
        InstantiateEnemyArgs args =  JsonUtility.FromJson<InstantiateEnemyArgs>(jsonValue);
        InitiateEnemy(args.data);
    }

    public void InitiateAlly(AllyData data){
        if(data==null){
            Debug.LogWarning("没有对应的CharData");
            return;
        }
        GameObject Obj_char = InitiateEntity(data.Prefab);
        if (Obj_char == null) return;
        Ally charController = Obj_char.AddComponent<Ally>();        
        CharacterAttribute charAttr = Obj_char.AddComponent<CharacterAttribute>();    
        if(parent != null){
            Obj_char.transform.SetParent(parent);
        }

        //传入初始敌方友军列表
        charController.SetCharacterAbility(new AbilityController(charController, List_Enemy, List_Ally));
        //注意一定要确保所有模块都添加好了再Init()
        charController.Init(data);
        Dic_ICharController.Add(Obj_char.GetInstanceID(), charController);
        
    }


     public void InitiateEnemy(EnemyData data){
        if(data==null){
            Debug.LogWarning("没有对应的EnemyData");
             return;
        }
        GameObject Obj_char = InitiateEntity(data.Prefab);
        if (Obj_char == null) return;
        Enemy enemyController = Obj_char.AddComponent<Enemy>();
        EnemyAttribute charAttr = Obj_char.AddComponent<EnemyAttribute>(); 
        //传入初始敌方友军列表
        enemyController.SetCharacterAbility(new AbilityController(enemyController, List_Enemy, List_Ally));  
        //注意一定要确保所有模块都添加好了再Init()      
        enemyController.Init(data);
        Dic_ICharController.Add(Obj_char.GetInstanceID(), enemyController);
        
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
        EntityComponetsControllerUtils.AddAllComponents(setupData, obj);
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
    public void RemoveEntityWithValue(Entity IChar){
        if(!Dic_ICharController.ContainsValue(IChar)){
            Debug.LogWarning(this.name+"Dic_ICharController没有value为该IChar的元素");
            return;
        }
        
    }

    /*
    @function 在字典中返回为对应值的索引
    */
    public int GetID(Entity IChar){
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
    public Entity GetEntity(int ID){
        return Dic_ICharController[ID];
    }

    /*
    @function 获取对应obj的Icharacter
    */
    public Entity GetEntity(GameObject obj){
        return GetEntity(obj.GetInstanceID());
    }

}

public class InstantiateAllyArgs : EventArgs{
    public AllyData data;
    public InstantiateAllyArgs(AllyData data){
        this.data = data;
    }
}


public class InstantiateEnemyArgs : EventArgs{
    public EnemyData data;
    public InstantiateEnemyArgs(EnemyData data){
        this.data = data;
    }
}