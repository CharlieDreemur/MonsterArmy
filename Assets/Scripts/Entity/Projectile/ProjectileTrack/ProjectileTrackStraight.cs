using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrackStraight : AbstractProjectileTrack
{   
    public ProjectileTrackStraight(Projectile projectile):base(projectile){
        projectile.RB.bodyType = RigidbodyType2D.Kinematic;
        float angle = Mathf.Atan2(projectile.args.direction.y, projectile.args.direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void Update()
    {
        base.Update();
        float step = Speed * Time.fixedDeltaTime;
        Vector3 displacement = projectile.args.direction * step;
        projectile.transform.position += displacement;
        projectile.transform.position = Vector3.Lerp(projectile.transform.position, projectile.transform.position+displacement, step);
        projectile.distance += step;
    }

    /*
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
    */
}
