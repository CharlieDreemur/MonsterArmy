using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 不同计算projectile的移动轨迹的算法
/// </summary>
public abstract class IProjectileTrack
{
    [ShowInInspector]
    protected float velocity;
    protected Projectile projectile;
    protected Rigidbody2D rigidBody2D;

    public IProjectileTrack(Projectile projectile){
        this.projectile = projectile;
        velocity = projectile.GetProjectileData().velocity;
        rigidBody2D = projectile.GetRigidBody2D();
     
        
    }

    public virtual void Shoot(){
        //移动到起始点
        
    }

    public virtual void Update(){

    }

}
