using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//auto-set the components for all characters that spawn
[CreateAssetMenu(fileName = "SetupData", menuName = "TheMonsterArmy/ScriptableObject/SetupData", order = 0)]
public class SetupData : ScriptableObject{
    public bool isHealthBar;
    public bool isRigidBody2D;
    public bool isCapsuleCollider2D;
    public Vector2 CapsuleCollider2D_offset;
    public Vector2 CapsuleCollider2D_size;

    public HealthBarData healthBarData;
}
