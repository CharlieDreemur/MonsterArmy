using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStraightTrack : IProjectileTrack
{   
    [SerializeField]
    private Vector3 targetPos;

    [SerializeField]
    private Vector3 attackPos;

    [SerializeField]
    private Vector3 projectilePos;
    [SerializeField]
    private Vector3 v;
    
    public ProjectileStraightTrack(Projectile projectile):base(projectile){

    }
    public override void Shoot(){
        base.Shoot();
        rigidBody2D.gravityScale =  0f;
        targetPos = projectile.GetTarget().GetPosition() + projectile.GetRelativePos();
        targetPos.y += projectile.GetProjectileData().heightCorrection;
        projectilePos = projectile.GetPosition();
        attackPos = projectile.GetAttacker().GetPosition();
        v = UtilsClass.GetPosToDirection(attackPos, targetPos);
        v.z = 0; //2D游戏确保z轴为0
        float angle = Vector3.SignedAngle(Vector3.up,v,Vector3.forward); 
        Quaternion rotation = Quaternion.Euler(0, 0, angle+ 90f);    //利用角度得到rotation, +90f是因为0度向右
        projectile.transform.rotation = rotation;
        rigidBody2D.velocity = new Vector2(v.x *velocity, v.y * velocity);
        
    }

    public override void Update(){
        //projectile.transform.position = Vector3.MoveTowards(projectile.GetPosition(), targetPos, velocity*Time.deltaTime);
        
        
    }
}
