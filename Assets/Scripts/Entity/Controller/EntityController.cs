using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理所有的ICharacter，负责ICharacter的生成和更新,ICharacter的对象池，之后会考虑将这两个功能分离并增加characterFactory
public class EntityController:MonoBehaviour
{   
    static public EntityController Instance{get; private set;}
    public SetupData setupData;
    [Header("Set in the Inspector")]
    [Header("Set Dynamically")]

    public List<FriendData> List_FriendData = new List<FriendData>();
    public List<EnemyData> List_EnemyData = new List<EnemyData>();
    public Dictionary<int, ICharacter> Dic_ICharController = new Dictionary<int, ICharacter>();
    public List<ICharacter> List_Friend = new List<ICharacter>(); //玩家方所有单位的List
    public List<ICharacter> List_Enemy = new List<ICharacter>(); //敌人方所有单位的List

    public Transform parent;
    // Start is called before the first frame update
    private void Awake()
    {   
        Init();
        for(int i = 0; i<List_FriendData.Count;i++){
            InitiateFriend(i);
        }

        for(int i = 0; i<List_EnemyData.Count;i++){
            InitiateEnemy(i);
        }
    }
    void Start(){
        
    }
    // Update is called once per frame
    void Update()
    {

        //遍历字典的ICharacter并更新
        foreach(ICharacter item in Dic_ICharController.Values)
        {
            SetDictionaryToList(Dic_ICharController);
            //更新目标
            switch (item.type_Character){
                case Enum_Character.Character:
                    item.GetCharacterAbility().UpdateTargets(List_Enemy, List_Friend);
                    break;
                case Enum_Character.Enemy:
                    item.GetCharacterAbility().UpdateTargets(List_Friend, List_Enemy);
                    break;
            }
            //item.Update();
        }

    }

    public void Init(){
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //将Dic_ICharController按照阵营添加进不同的List
    public void SetDictionaryToList(Dictionary<int, ICharacter> _Dic_ICharController){
        List_Friend.Clear();
        List_Enemy.Clear();
        foreach(ICharacter item in _Dic_ICharController.Values){
            switch (item.type_Character){
                case Enum_Character.Character:
                    List_Friend.Add(item as Friend);
                    break;
                case Enum_Character.Enemy:
                    List_Enemy.Add(item as Enemy);
                    break;
            }
         
        }
    }

    /*
    @function 实例化一个GameObject, 添加对应的Character和CharacterAI到字典
    */
    public void InitiateFriend(int index){
        if(List_FriendData[index]==null){
            Debug.LogWarning("没有对应的CharData");
            return;
        }
        GameObject Obj_char = InitiateCharacter(List_FriendData[index].Prefab, index);
        
        //Character charController = new Character(Obj_char, List_CharData[index]);
        Friend charController = Obj_char.AddComponent<Friend>();        
        CharacterAttribute charAttr = Obj_char.AddComponent<CharacterAttribute>();    
        if(parent != null){
            Obj_char.transform.SetParent(parent);
        }

        //传入初始敌方友军列表
        charController.SetCharacterAbility(new ICharacterAbility(charController, List_Enemy, List_Friend));
        //注意一定要确保所有模块都添加好了再Init()
        charController.Init(List_FriendData[index]);
        Dic_ICharController.Add(Obj_char.GetInstanceID(), charController);
        
    }


    public void InitiateEnemy(int index){
        if(List_EnemyData[index]==null){
            Debug.LogWarning("没有对应的EnemyData");
             return;
        }
        GameObject Obj_char = InitiateCharacter(List_EnemyData[index].Prefab, index);
        //Enemy enemyController = new Enemy(Obj_char, List_EnemyData[index]);
        Enemy enemyController = Obj_char.AddComponent<Enemy>();
        EnemyAttribute charAttr = Obj_char.AddComponent<EnemyAttribute>(); 
        //传入初始敌方友军列表
        enemyController.SetCharacterAbility(new ICharacterAbility(enemyController, List_Enemy, List_Friend));  
        //注意一定要确保所有模块都添加好了再Init()      
        enemyController.Init(List_EnemyData[index]);
        Dic_ICharController.Add(Obj_char.GetInstanceID(), enemyController);
        
    }

    //上面两个的公用部分
    public GameObject InitiateCharacter(GameObject prefab, int index){
        if(prefab == null){
            Debug.LogWarning(this.name+"没有对应index的prefabs");
            return null;
        }
        GameObject obj = Instantiate(prefab);
        if(parent != null){
            obj.transform.SetParent(parent);
        }
        CharacterComponetsController.AddAllComponents(setupData, obj);
        return obj;
    }
    /*
    @function 删除一个索引为指定ID/Key的元素
    */
    public void RemoveCharacterWithID(int ID){
        if(!Dic_ICharController.ContainsKey(ID)){
            Debug.LogWarning(this.name+"Dic_ICharController没有key为该ID的元素");
            return;
        }
        Dic_ICharController.Remove(ID);
    }

      /*
    @function 删除一个索引为指定ID/Key的元素
    */
    public void RemoveCharacterWithValue(ICharacter IChar){
        if(!Dic_ICharController.ContainsValue(IChar)){
            Debug.LogWarning(this.name+"Dic_ICharController没有value为该IChar的元素");
            return;
        }
        
    }

    /*
    @function 在字典中返回为对应值的索引
    */
    public int GetID(ICharacter IChar){
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
    public ICharacter GetCharacter(int ID){
        return Dic_ICharController[ID];
    }

    /*
    @function 获取对应obj的Icharacter
    */
    public ICharacter GetCharacter(GameObject obj){
        return GetCharacter(obj.GetInstanceID());
    }

    public void PrintDic(){
        foreach(int key in Dic_ICharController.Keys){
            Debug.Log(key+":"+Dic_ICharController[key]+" ");
        }
    }
}
