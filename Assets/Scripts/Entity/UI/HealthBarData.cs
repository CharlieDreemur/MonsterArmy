using System.Collections;
using System.Collections.Generic;
using MonsterArmy.Core.UnitSystem.Interface;
using UnityEngine;
namespace MonsterArmy.Core.UnitSystem.Data
{
    [CreateAssetMenu(fileName = "HealthBarData", menuName = "TheMonsterArmy/ScriptableObject/InfoUI/HealthBarData", order = 0)]

    public class HealthBarData : ScriptableObject, IUnitComponentInitData
    {
        public UnityEngine.GameObject prefab;
        public Vector3 scale = Vector3.one;
        public Vector3 relativePos = Vector3.zero;

        public UnityEngine.GameObject Create()
        {
            UnityEngine.GameObject obj = Instantiate(prefab) as UnityEngine.GameObject;
            obj.transform.localScale = scale;
            obj.transform.position = relativePos;
            return obj;
        }
    }

}
