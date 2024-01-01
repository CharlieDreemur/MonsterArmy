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

    
}
