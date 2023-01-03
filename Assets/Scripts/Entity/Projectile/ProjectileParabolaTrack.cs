using System.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class ProjectileParabolaTrack : IProjectileTrack
{
    [SerializeField]
    private Vector2 targetPos;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float velocityfourthpower; 
    [SerializeField]
    private float velocity_x;
    [SerializeField]
    private float velocity_y;
    [SerializeField]
    private Vector2 finalvelocity;
    [SerializeField]
    private float distance_x;
    [SerializeField]
    private float distance_y;
    [SerializeField]
    private float radian;
    [SerializeField]
    private float tan;

    [SerializeField]
    private Entity attacker;

    [SerializeField]
    private Entity target;

    [SerializeField]
    private Vector3 projectilePos;

    public ProjectileParabolaTrack(Projectile projectile):base(projectile){
        Init();
    }    

    public void Init(){
        attacker = projectile.GetAttacker();
        target = projectile.GetTarget();
        rigidBody2D.gravityScale =  projectile.GetProjectileData().gravityScale;
        gravity = projectile.GetProjectileData().gravityScale * -9.8f;
        velocityfourthpower = Mathf.Pow(velocity, 4);
        projectilePos = projectile.GetPosition();
        targetPos = projectile.GetTarget().GetPosition();
        targetPos.y+=projectile.GetProjectileData().heightCorrection; //高度补正
        distance_x = Mathf.Abs(targetPos.x - projectilePos.x);
        distance_y = Mathf.Abs(targetPos.y - projectilePos.y);
        float xsquare = Mathf.Pow(distance_x , 2);
        float ysquare = Mathf.Pow(distance_y , 2);
        float sqrt = velocityfourthpower-gravity*(gravity*xsquare+2*distance_y*velocity*velocity);
        if(sqrt<0){
            velocity_x = velocity/1.41421356237f;
            velocity_y = velocity/1.41421356237f;
            finalvelocity = new Vector2(velocity_x, velocity_y);
            return;
        }
        float c = Mathf.Sqrt(sqrt);
        if(projectile.GetProjectileData().isGreaterAngle){
            tan = (velocity*velocity + c)/(gravity*distance_x);
        }
        else{
            tan = (velocity*velocity - c)/(gravity*distance_x);
        }
        radian = Mathf.Atan(tan);
        velocity_x = Mathf.Cos(radian) * velocity;
        velocity_y = -Mathf.Sin(radian) * velocity;
        finalvelocity = new Vector2(velocity_x, velocity_y);
    }


    

    public override void Shoot(){
        base.Shoot();
        if(projectile.GetAttacker().characterDirection == Enum_CharacterDirection.right){
            //弓箭向右
            rigidBody2D.velocity = finalvelocity; 
        }
        else{
            //弓箭向左
            rigidBody2D.velocity = new Vector2(-finalvelocity.x, finalvelocity.y);
        }
       
    }

    public override void Update(){
       
        float angle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
