using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 不同计算projectile的移动轨迹的算法
/// </summary>
[System.Serializable]
public abstract class AbstractProjectileTrack
{
    protected ProjectileData data;
    protected Projectile projectile;
    protected Unit target;
    protected float speedMultipler;
    protected AnimationCurve speedCurve;
    protected float Speed
    {
        get
        {
            return speedMultipler * speedCurve.Evaluate(projectile.time);
        }
    }
    protected Vector3 Scale
    {
        get
        {
            return data.scale * data.scaleCurve.Evaluate(projectile.time);
        }
    }
    public AbstractProjectileTrack(Projectile projectile)
    {
        this.projectile = projectile;
        this.data = projectile.args.Data;
        this.target = projectile.args.target;
        this.speedMultipler = data.speedMultipler;
        this.speedCurve = data.speedCurve;
        projectile.RB.gravityScale = data.gravityScale;
    }
    public virtual void Update()
    {
        projectile.transform.localScale = Scale;
        if (projectile.ETFX != null)
        {
            projectile.ETFX.SetScale(projectile.args.Data.scale);
        }
    }


}
