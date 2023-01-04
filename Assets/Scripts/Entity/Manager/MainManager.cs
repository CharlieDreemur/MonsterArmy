using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>, IManager
{
    //游戏主系统，控制其他所有的系统
    DamageTextManager damageTextManager;
    EntityManager entityManager;
    EventManager eventManager;
    ProjectileManager projectileManager;
    protected override void OnAwake()
    {
        eventManager = GetComponent<EventManager>();
        damageTextManager = GetComponent<DamageTextManager>();
        entityManager = GetComponent<EntityManager>();
        projectileManager = GetComponent<ProjectileManager>();
    }

    void Start(){
        Init();
    }
    public void Init(){
        eventManager.Init();
        damageTextManager.Init();
        entityManager.Init();
        projectileManager.Init();
    }
    
    void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            MessageManager.AddMessage("测试消息");
        }
        entityManager.OnUpdate();
    }

    void OnApplicationQuit() {
        //退出软件时调用
    }
}
