using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MonsterArmy.Core
{
    public class FightManager : Singleton<FightManager>, IManager
    {
        //System Related to Fight
        DamageTextManager damageTextManager;
        UnitManager unitManager;
        EventManager eventManager;
        ProjectileManager projectileManager;
        protected override void OnAwake()
        {
            eventManager = GetComponent<EventManager>();
            damageTextManager = GetComponent<DamageTextManager>();
            unitManager = GetComponent<UnitManager>();
            projectileManager = GetComponent<ProjectileManager>();
        }

        void Start()
        {
            Init();
        }
        public void Init()
        {
            eventManager.Init();
            damageTextManager.Init();
            unitManager.Init();
            projectileManager.Init();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                MessageManager.AddMessage("测试消息");
            }
            unitManager.OnUpdate();
        }

        void OnApplicationQuit()
        {
            //退出软件时调用
        }
    }
}