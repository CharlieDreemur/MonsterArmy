using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CollideData", menuName = "TheMonsterArmy/CollideSystem/CollideData", order = 0)]
public class CollideData : ScriptableObject
{
    [Header("Attack")]
    public string attackName;
    public GameObject prefab;
    public float attackDuration;
    public int maxHitCount = -1;
    public bool followInstigator;

    [Header("Shape")]
    public AttackShape attackShape;

    [Header("Circle Settings")]
    [Min(0.0f)]
    public float radius;
    [Range(0.0f, 360.0f)]
    public float sectorAngle;

    [Header("Box Settings")]
    public Vector2 boxSize;

    [Header("Misc")]
    public float totalDuration;
    public int damageOverTimeHitCount = -1;
    public GameObject effect = null;
    public float effectScale = 1.0f;
    public AudioClip sfx = null;
    public AudioClip hitSound = null;
}