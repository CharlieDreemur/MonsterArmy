using System.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class ProjectileTrackParabola : AbstractProjectileTrack
{
    [SerializeField]
    private Vector2 targetPos;
    [SerializeField]
    private float g;

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
    private float angle;
    [SerializeField]
    private float tan;

    [SerializeField]
    private Vector3 projectilePos;

    public ProjectileTrackParabola(Projectile projectile):base(projectile){
        projectile.RB.bodyType = RigidbodyType2D.Dynamic;
        Init();
    }    

    
    public void Init(){
        //should only use fixed speed here, so speed multipler only
        float v = speedMultipler;
        projectile.RB.gravityScale =  projectile.args.Data.gravityScale;
        g = projectile.args.Data.gravityScale * -9.8f;
        if(g == 0){
            Debug.LogWarning("gravity should not be zero");
            return;
        }
        projectilePos = projectile.transform.position;
        if(projectile.args.target == null){
            Debug.LogWarning("target is null");
            return;
        }
        targetPos = projectile.args.target.transform.position;
        //targetPos.y+=projectile.GetProjectileData().heightCorrection; //高度补正
        distance_x = - (projectilePos.x - targetPos.x);
        distance_y = projectilePos.y - targetPos.y;
        float xsquare = Mathf.Pow(distance_x , 2);
        float ysquare = Mathf.Pow(distance_y , 2);
        float sqrt = (v*v*v*v)-g*(g*xsquare+2*distance_y*v*v);
        if(sqrt<0){
            velocity_x = v/1.41421356237f;
            velocity_y = v/1.41421356237f;
            finalvelocity = new Vector2(velocity_x, velocity_y);
            Debug.LogWarning("Too Far Away for shoot");
            return;
        }
        float c = Mathf.Sqrt(sqrt);
        if(projectile.args.Data.isGreaterAngle){
            tan = (v*v + c)/(g*distance_x);
        }
        else{
            tan = (v*v - c)/(g*distance_x);
        }
        radian = Mathf.Atan(tan);
        angle = radian * Mathf.Rad2Deg;
        velocity_x = Mathf.Cos(radian) * v;
        velocity_y = -Mathf.Sin(radian) * v;
        finalvelocity = new Vector2(velocity_x, velocity_y);
        if(projectile.args.damageInfo.attacker.characterDirection == Enum_CharacterDirection.right){
            //弓箭向右
            projectile.RB.velocity = finalvelocity; 
        }
        else{
            //弓箭向左
            projectile.RB.velocity = new Vector2(-finalvelocity.x, -finalvelocity.y);
        }
    }
    
    public override void Update(){
        base.Update();
        float angle = Mathf.Atan2(projectile.RB.velocity.y, projectile.RB.velocity.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
   
}
