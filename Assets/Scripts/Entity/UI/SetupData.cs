using System.Collections;
using System.Collections.Generic;
using MonsterArmy.Core.UnitSystem.Interface;
using UnityEngine;

//auto-set the components for all characters that spawn
namespace MonsterArmy.Core.UnitSystem.Data
{
    [CreateAssetMenu(fileName = "SetupData", menuName = "TheMonsterArmy/ScriptableObject/SetupData", order = 0)]
    public class SetupData : ScriptableObject, IUnitComponentInitData
    {
        public bool isHealthBar;
        public bool isRigidBody2D;
        public bool isCapsuleCollider2D;
        public Vector2 CapsuleCollider2D_offset;
        public Vector2 CapsuleCollider2D_size;

        public HealthBarData healthBarData;
        public Material material;
    }
}