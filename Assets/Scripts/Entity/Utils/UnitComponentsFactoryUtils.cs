using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterArmy.Core.UnitSystem.Interface;
using MonsterArmy.UI;
using MonsterArmy.Core.UnitSystem.Data;
namespace MonsterArmy.Core.UnitSystem
{
    //control all the componets that will be auto setup to each characters
    public static class UnitComponentsFactoryUtils
    {
        public static void AddAllComponents(SetupData data, GameObject obj)
        {
            //必须添加组件
            if (obj == null)
            {
                Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + "gameobject does not exist");
                return;
            }
            TryAddComponent<UnitAttributeController>(obj);
            TryAddComponent<UnitAnimationController>(obj);
            SetMaterial(data, obj);
            //add attack component
            if (obj.GetComponent<RangedAttackComponent>() == null)
                TryAddComponent<AttackComponent>(obj);

            //可选添加组件
            if (data == null)
            {
                Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + "setup data does not exist");
                return;
            }
            TryAddComponent<HealthBarController>(obj, data.healthBarData);
            AddRigidBody2D(data, obj);
            AddCapsuleCollider2D(data, obj);
            //Debug.Log(obj.name+"组件添加成功");
        }
        public static T TryAddComponent<T>(GameObject obj, IUnitComponentInitData initData = null) where T : MonoBehaviour, IUnitComponent
        {
            if(obj == null)
            {
                Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + "gameobject does not exist");
                return null;
            }
            //TryGetComponent
            obj.TryGetComponent<T>(out T component);
            if (component == null)
            {
                component = obj.AddComponent<T>();
            }
            initData = initData ?? new DeafultUnitComponentInitData();
            component.Init(initData);
            return component;
        }


        public static void AddRigidBody2D(SetupData data, GameObject character)
        {
            if (!data.isRigidBody2D)
            {
                return;
            }
            //CheckComponents
            if (character.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + "components already exist");
                return;
            }
            Rigidbody2D toReturn = character.gameObject.AddComponent<Rigidbody2D>();
            SetRigidBody2D(toReturn);

        }

        public static void AddCapsuleCollider2D(SetupData data, GameObject character)
        {
            if (!data.isRigidBody2D)
            {
                return;
            }
            //CheckComponents
            if (character.gameObject.GetComponent<CapsuleCollider2D>() != null)
            {
                Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + "components already exist");
                return;
            }
            CapsuleCollider2D toReturn = character.gameObject.AddComponent<CapsuleCollider2D>();
            SetCapsuleCollider2D(toReturn, data);

        }


        public static void SetRigidBody2D(Rigidbody2D obj)
        {
            if (obj == null)
            {
                Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + "obj does not exist");
                return;
            }
            obj.bodyType = RigidbodyType2D.Kinematic;
        }

        public static void SetCapsuleCollider2D(CapsuleCollider2D obj, SetupData data)
        {
            if (obj == null)
            {
                Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + "obj does not exist");
                return;
            }
            obj.offset = data.CapsuleCollider2D_offset;
            obj.size = data.CapsuleCollider2D_size;
        }

        public static void SetMaterial(SetupData data, GameObject obj)
        {
            SpriteRenderer[] spriteRenderers = obj.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer item in spriteRenderers)
            {
                item.material = data.material;
            }
        }
    }
}