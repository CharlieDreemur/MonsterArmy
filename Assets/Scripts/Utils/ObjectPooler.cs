using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler:MonoBehaviour
{
    public Transform commonParent; //这个为公共parent，每个具体的pool有自己的localParent，如果没设置localParent则直接放进parent里

    [System.Serializable]
    public class Pool{

        
        public IDataWithPrefab data;
        public Queue<UnityEngine.GameObject> queue = new Queue<UnityEngine.GameObject>();
        public bool isLocalParent = false;
        public Transform localParent;
    
        public int preSize; //池初始数量
        public int freeObjectCount = 0; //对象池可用剩余对象数量
    
        public UnityEngine.GameObject Spawn(UnityEngine.GameObject prefab, Vector3 position, Quaternion rotation){
            UnityEngine.GameObject objectToSpawm;
            //如果队列的所有物体都已经被激活，则扩充对象池容量，新建一个对象
            if(queue.Count>0){
            objectToSpawm = queue.Dequeue();
            freeObjectCount=queue.Count;
            }
            else{
                objectToSpawm = Instance.CreateGameObject(prefab);
            }

            objectToSpawm.SetActive(true);
            SetPrefabParent(objectToSpawm);
            objectToSpawm.transform.position = position;
            objectToSpawm.transform.rotation = rotation;
            return objectToSpawm;
        }


        /// <summary>
        /// 当objectpool里的对象触发OnObjectDestroyed()时调用
        /// </summary>
        public void Recycle(UnityEngine.GameObject prefab, Transform parent){
        
            //该分配对象已经在对象池中
            if(queue.Contains(prefab)){
                Debug.LogWarning("THE GameObject "+prefab.name+" has already exist");
                return;
            }
            queue.Enqueue(prefab);
            SetPrefabParent(prefab);
            
            freeObjectCount = queue.Count;
            
        }

        public void PrintQueue(){
            string str = "Queue:";
            foreach(var item in queue){
                str+=item.ToString()+" ";
            }
            Debug.Log(str);
        }

        private void SetPrefabParent(UnityEngine.GameObject prefab){
              if(isLocalParent){
                prefab.transform.SetParent(localParent);
            }
            else{
                prefab.transform.SetParent(Instance.commonParent);
            }
        }
    }

    public static ObjectPooler Instance{get; private set;}
    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }  

    private void Start() {
        Init();
    }
    public List<Pool> pools;
    public Dictionary<IDataWithPrefab, Pool> poolDictionary;
    

    /// <summary>
    /// 初始化对象池所有对象
    /// </summary>
    public void Init()
    {
        poolDictionary = new Dictionary<IDataWithPrefab, Pool>();
        foreach(Pool pool in pools){
            
            for(int i = 0; i < pool.preSize; i++){
                //Debug.Log(pool.data.Prefab);
                AddContainer(pool.queue, pool.data.Prefab);
                
            }
            pool.freeObjectCount = pool.preSize; //设置剩余对象数量为池初始数量
            poolDictionary.Add(pool.data, pool);
        }
        
    }

    /// <summary>
    /// 增加对象池的容量,返回最新增加的prefab
    /// </summary>
    public void AddContainer(Queue<UnityEngine.GameObject> queue, UnityEngine.GameObject prefab){
        UnityEngine.GameObject obj = CreateGameObject(prefab);
        queue.Enqueue(obj);
    
    }

    public UnityEngine.GameObject CreateGameObject(UnityEngine.GameObject prefab){
        UnityEngine.GameObject obj = Instantiate(prefab);
        obj.transform.SetParent(commonParent);
        obj.SetActive(false);
        //Debug.Log("Create new GameObject"+obj.name);
        return obj;
    }

   
    /// <summary>
    /// 从对象池中创建对象
    /// </summary>
    /// <param name="data"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public UnityEngine.GameObject Spawn(IDataWithPrefab data, Vector3 position, Quaternion rotation){

        if(!poolDictionary.ContainsKey(data)){
            Debug.LogWarning("Pool with prefab "+data+"doesn't exist");
            
            return null;
        }
        Pool objectPool = poolDictionary[data];
        UnityEngine.GameObject objectToSpawm = objectPool.Spawn(data.Prefab, position, rotation);

        //在每次重新从object pool调用object时做一次初始化
        // IPooledObject pooledObj = objectToSpawm.GetComponent<IPooledObject>();
        // if(pooledObj != null){
        //     pooledObj.OnObjectSpawn();
        // }
        
        return objectToSpawm;
    }

    public void Recycle(IDataWithPrefab data, UnityEngine.GameObject prefab){
        prefab.SetActive(false);
        if(!poolDictionary.ContainsKey(data)){
            Debug.LogWarning("Recycle:Pool with prefab "+data+"doesn't exist");
            return;
        }
        poolDictionary[data].Recycle(prefab, commonParent);
    }


  }
