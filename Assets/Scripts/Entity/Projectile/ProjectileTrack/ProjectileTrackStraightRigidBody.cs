using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrackStraightRigidBody : AbstractProjectileTrack
{   
    private Vector2 initialForce;
    private Rigidbody2D rb;

    public ProjectileTrackStraightRigidBody(Projectile projectile):base(projectile)
    {
        rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = projectile.gameObject.AddComponent<Rigidbody2D>();
        }

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0; // Assuming you don't want gravity to affect the projectile

        // Calculate initial force based on direction and speed
        initialForce = projectile.args.direction.normalized * Speed;
        
        // Set the rotation of the projectile
        float angle = Mathf.Atan2(projectile.args.direction.y, projectile.args.direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void Start()
    {
        base.Start();
        // Apply the initial force
        rb.AddForce(initialForce, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        base.Update();
        // Update distance based on Rigidbody's position
        projectile.distance = rb.position.magnitude;
    }

    
}
