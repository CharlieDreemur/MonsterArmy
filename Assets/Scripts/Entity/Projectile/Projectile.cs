using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Projectile : MonoBehaviour, IPooledObject
{   
    public Character_OnShoot EventOnShoot;
    private Vector3 direction;

    [ShowInInspector]
    private IProjectileTrack projectileTrack;
    private int ID;
    private Rigidbody2D rigidBody2D;
    private ETFXProjectile ETFX ;
    private Tween tween; //DoTween的进程

    /// <summary>
    /// Awake里的只会触发一次，不会在每次重新生成的时候再触发
    /// </summary>
    public void Awake(){
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        ETFX = GetComponent<ETFXProjectile>();
    }
    public void OnObjectSpawn(){
        StartCoroutine("LifeCycleTimer", GetProjectileData().lifeCycle); //启动生命周期计时器
        //Action_projectile();
        if(projectileTrack == null){
            Debug.LogWarning("ProjectileTrack does not exist");
            return;
        }
        if(ETFX!=null){
            ETFX.Init();
        }
        
        projectileTrack.Shoot();
       
    }

    public void Update(){
        if(projectileTrack == null){
            Debug.LogWarning("ProjectileTrack does not exist");
            return;
        }
        projectileTrack.Update();
    }
    public void Init(Character_OnShoot e){

        EventOnShoot = e;
        ID = EntityController.Instance.GetID(GetTarget());
        if(ETFX !=null){
            ETFX.SetScale(GetProjectileData().scale);
        }
        else{
            transform.localScale = GetProjectileData().scale;
        }


        //GetComponent<SpriteRenderer>().color = GetProjectileData().color;
        Vector3 projectilePos = GetAttacker().GetCharacterData().relativePos;
        switch (GetProjectileData().projectileType)
        {
            case Enum_ProjectileType.parabola:
            projectileTrack = new ProjectileParabolaTrack(this);

            break;

            case Enum_ProjectileType.straight:
            projectileTrack = new ProjectileStraightTrack(this);
            break;

            case Enum_ProjectileType.track:

            break;
            
            default:
            //Action_projectile = Shoot_Straight;
            break;
        }
        
    }


    #region Action_projectile



    
    /*
    /// <summary>
    /// 按照贝塞尔抛物线向目标位置射击，不跟踪
    /// </summary>
    private void Shoot_Parabola(){
        int resolution = 50;
        Vector3 startPos = Vector3.zero;
        Vector3 endPos = target.GetPosition();

        float height = 0f;
        if(projectileData.isAutoHeight){
            height = DistanceToTarget * 0.5f;
        }
        else{
            height = projectileData.height;
        }
        
        Vector3 bezierCenterPoint = (startPos+endPos)*0.5f +(Vector3.up*height);
        Vector3[] path = new Vector3[resolution];//resolution为int类型，表示要取得路径点数量，值越大，取得的路径点越多，曲线最后越平滑
        for (int i = 0; i < resolution; i++)
    {
        var t = (i+1) / (float)resolution;//归化到0~1范围
        path[i] = GetBezierPoint(t,startPos, bezierCenterPoint, endPos);//使用贝塞尔曲线的公式取得t时的路径点
    }
        tween = transform.DOPath(path, duration).SetEase(Ease.InOutQuad).SetLookAt(0, Vector3.left);
        
    }

     /// <param name="t">0到1的值，0获取曲线的起点，1获得曲线的终点</param>
    /// <param name="start">曲线的起始位置</param>
    /// <param name="center">决定曲线形状的控制点</param>
    /// <param name="end">曲线的终点</param>
    public static Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
    }

    */
    #endregion

   

    public void SetDirection(Vector3 _direction){
        direction = _direction;
    }

    public void EulerRotate(Vector3 _direction){
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(_direction));
    }
  

    private void OnTriggerEnter2D(Collider2D other) {
       
        if(!other.CompareTag("Character")){
            return;
        }
        if(ID==other.gameObject.GetInstanceID()){
            //TODO: 如果两个敌人并排，瞄准前方敌人的弓箭刚好打中后方的敌人，是否结算伤害？
            //Debug.Log("arrow_attack"+target);
            
            if(ETFX!=null){
                ETFX.HitImpact();
            }
            EventOnShoot.GetTarget().UnderAttack(GetAttacker());
            OnObjectRecycle();
        }
        
    }

   
    public ProjectileData GetProjectileData(){
        return EventOnShoot.GetProjectileData();
    }   

    public Vector3 GetPosition(){
        return transform.position;
    }

    public ICharacter GetTarget(){
        return EventOnShoot.GetTarget();
    }

    public ICharacter GetAttacker(){
        return EventOnShoot.GetAttacker();
    }

    public Vector3 GetRelativePos(){
        return EventOnShoot.GetAttacker().GetCharacterData().relativePos;
    }
    public Rigidbody2D GetRigidBody2D(){
        return rigidBody2D;
    }

    /// <summary>
    /// 经过生命周期后自动回收
    /// </summary>
    /// <returns></returns>
    IEnumerator LifeCycleTimer(float second){
        yield return new WaitForSeconds(second);
        OnObjectRecycle();
    }

    /// <summary>
    /// projectile的回收处理
    /// </summary>
    public void OnObjectRecycle(){
        StopCoroutine("LifeCycleTimer"); //终止生命周期计时器
        ObjectPooler.Instance.Recycle(GetProjectileData(), gameObject);
        tween.Kill();
    } 
    private void OnDestroy() {
        tween.Kill();
    }
}
