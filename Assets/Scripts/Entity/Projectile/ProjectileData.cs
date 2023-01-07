using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "ProjectileData", menuName = "TheMonsterArmy/ProjectileSystem/ProjectileData", order = 0)]

[System.Serializable]
public class ProjectileData:ScriptableObject, IPoolData{

    public string Name;
    public GameObject prefab;
    public GameObject Prefab{get => prefab; set => prefab = value;}
    public bool isAreaAttack = false;
    [ShowIf("isAreaAttack", true)]
    public CollideData collideData;

    [Min(0.0f)] [Tooltip("The projectile will be deleted after the lifeCycle run out")]
    public float lifeCycle = 5.0f; //The projectile will destory itself after the lifeCycle
    [Min(0.0f)]
    public float maxDistance = 10.0f;
    public ProjectileTrackType trackType;
    public Vector3 scale = Vector3.one; //Scale of Projectile
    public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe(1, 1), new Keyframe(1, 1));
    public float speedMultipler = 1;
    public AnimationCurve speedCurve = new AnimationCurve(new Keyframe(1, 1), new Keyframe(1, 1));
    //The final speed will be the speed*speedCurve

    [Tooltip("If yes, the projectile will call trigger() when it is released")]
    public bool isTriggerWhenRelease = true;
    
    [Space(10)]
    [Header("TrackParabola Setup")]
    public float gravityScale;
    public bool isGreaterAngle;

}