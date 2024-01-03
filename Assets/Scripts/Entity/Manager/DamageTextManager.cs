using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using MonsterArmy.UI;
namespace MonsterArmy.Core
{
    public class DamageTextManager : Singleton<DamageTextManager>, IManager
    {
        private UnityAction<string> action;
        public static DamageTextData data;
        protected override void OnAwake()
        {
            data = Resources.Load("Data/UI/DamageTextData") as DamageTextData;
            action = new UnityAction<string>(Create);
        }

        public void Init()
        {
            EventManager.StartListening("CreateDamageText", action);
        }

        private void OnDisable()
        {
            EventManager.StopListening("CreateDamageText", action);
        }

        public static void Create(string jsonValue)
        {
            CreateDamageTextEventArgs args = JsonUtility.FromJson<CreateDamageTextEventArgs>(jsonValue);
            Create(args);
        }

        public static void Create(CreateDamageTextEventArgs args)
        {
            if (data == null)
            {
                Debug.LogWarning("没有提供伤害跳字的数据");
                //return null;
            }
            //GameObject damageTextGameObject = Instantiate(data.prefab, pos, Quaternion.identity);
            UnityEngine.GameObject damageTextGameObject = PoolManager.Spawn(data.Prefab, args.pos, Quaternion.identity);
            DamageText damageText = damageTextGameObject.GetComponent<DamageText>();
            Debug.Log("damageText:" + damageText);
            damageText.Init(data, args);
            damageText.OnSpawn();

            //return damageText;
        }

    }

    public class CreateDamageTextEventArgs : EventArgs
    {
        public CreateDamageTextEventArgs(Vector3 pos, int damageAmount, Enum_DamageType damageType, bool isCrit)
        {
            this.pos = pos;
            this.damageAmount = damageAmount;
            this.damageType = damageType;
            this.isCrit = isCrit;
        }
        public Vector3 pos;
        public int damageAmount;
        public Enum_DamageType damageType;
        public bool isCrit;
    }
}