using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using Sirenix.OdinInspector;

public enum ProjectileTrackType { straight, homing, parabola }

//WAY too much abstraction 

//Everything needed for instantiate a projectile that given not by projectile itself and need to set by outer classes
[System.Serializable]
public class ProjectileArgs : EventArgs
{
    public bool isInit;

    public ProjectileArgs(
        ProjectileData data,
        Vector3 spawnPos,
        DamageInfo damageInfo,
        Unit target
        )
    {
        this.data = data;
        this.spawnPos = spawnPos;
        this.damageInfo = damageInfo;
        this.target = target;
        this.direction = UtilsClass.GetPosToDirection(damageInfo.attacker.transform.position, target.transform.position);
        isInit = true;
    }
    public ProjectileArgs(
        ProjectileData data,
        Vector3 spawnPos,
        DamageInfo damageInfo,
        Vector3 direction,
        Unit target = null
        )
    {
        this.data = data;
        this.spawnPos = spawnPos;
        this.damageInfo = damageInfo;
        this.direction = direction;
        this.target = target;

        isInit = true;
    }

    public ProjectileData Data
    {
        get
        {
            if (data is null)
            {
                Debug.LogWarning("Warning: ProjectileArgs.data is null!");
            }
            return data;
        }
        set => data = value;
    }
    public DamageInfo damageInfo;
    public Vector3 spawnPos;
    public Vector3 direction;
    public Unit target; //homing target
    [SerializeField]
    private ProjectileData data;

    public Vector3 GetAttackerPosition()
    {
        return damageInfo.attacker.GetPosition();
    }

    public Vector3 GetTargetPosition()
    {
        return target.GetPosition();
    }

}


public class Projectile : MonoBehaviour, IPoolObject
{
    //Inheriter Base Values
    public bool isInit = false;
    public bool isTrigger = false; //You can't trigger it twice
    public ProjectileArgs args;
    [ShowInInspector]
    public AbstractProjectileTrack track;
    public ETFXProjectile ETFX;
    public float distance;
    public float time;
    public Rigidbody2D RB
    {
        get
        {
            if (rb is null)
            {
                Debug.LogWarning("Warning: ProjectileArgs.rigidbody is null!");
            }
            return rb;
        }
        set => rb = value;
    }
    [SerializeField]
    private Unit collideEntity;
    [SerializeField]
    private List<Unit> triggerEntities;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]



    public static GameObject InstantiateProjectile(ProjectileArgs projectileArgs)
    {
        GameObject attack = PoolManager.Spawn(projectileArgs.Data.prefab, projectileArgs.spawnPos, Quaternion.identity);
        Projectile projectileComponent = attack.GetComponent<Projectile>();
        projectileComponent.Init(projectileArgs);
        return attack;
    }

    public void Init(ProjectileArgs projectileArgs)
    {
        this.args = projectileArgs;
        //Only run after the cast is initialized
        if (!isInit || args.Data == projectileArgs.Data)
        {
            DataInit();
        }
        if (ETFX != null)
        {
            ETFX.Init();
        }
        isInit = true;
        OnEnable();
    }

    void Awake()
    {
        gameObject.tag = "Projectile";
        RB = GetComponent<Rigidbody2D>();
    }

    //Be Called Each time when spawn by PoolManger
    private void OnEnable()
    {
        if (!isInit)
        {
            return;
        }
        isTrigger = false;
        distance = 0f;
        time = 0f;
        triggerEntities = new List<Unit>();
        StartCoroutine(ReleaseObject(args.Data.lifeCycle));
    }

    private void OnDisable()
    {
        StopCoroutine("ReleaseObject"); //Since Already Release, Release Coroutine will be stopped
    }

    private void DataInit()
    {
        transform.localScale = args.Data.scale;
        if (ETFX != null)
        {
            ETFX.SetScale(args.Data.scale);
        }
        switch (args.Data.trackType)
        {
            case ProjectileTrackType.straight:
                track = new ProjectileTrackStraight(this);
                break;
            case ProjectileTrackType.parabola:
                track = new ProjectileTrackParabola(this);
                break;
            case ProjectileTrackType.homing:
                if (args.target is null)
                {
                    Debug.LogWarning("Homing Projectile while target is null");
                    return;
                }
                break;
        }
    }

    /// <summary>
    /// It will activate the event list one by one in the data
    /// </summary>
    public void Trigger()
    {
        isTrigger = true;
        if (args.Data.isAreaAttack)
        {
            CreateAreaAttack();
        }
        OnRelease();
    }

    void FixedUpdate()
    {
        if (!isInit)
        {
            return;
        }
        time += Time.fixedDeltaTime;
        if (distance > args.Data.maxDistance)
        {
            Release();
        }
        track.Update();
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (isTrigger) { return; }

        bool isEntity = collider.TryGetComponent(out Unit entity);
        if (!isEntity) { return; }
        if (entity == args.damageInfo.attacker)
        {
            Physics2D.IgnoreCollision(args.damageInfo.attacker.GetComponent<Collider2D>(), collider);
            return;
        }
        //Only attack enemy
        if (args.damageInfo.attacker.entityType == entity.entityType)
        {
            return;
        }
        /*
        if (args.ignoredCollisionList != null && args.ignoredCollisionList.Count > 0)
        {
            foreach (Entity item in args.ignoredCollisionList)
            {
                if (entity == item)
                {
                    Physics.IgnoreCollision(item.GetComponent<Collider>(), collider);
                    return;
                }
            }
        }
        */
        triggerEntities.Add(entity);
        collideEntity = entity;
        Trigger();
        if (!args.Data.isAreaAttack)
        {
            collideEntity.TakeDamage(args.damageInfo);
        }
    }

    //Release the projectile
    void Release()
    {
        if (args.Data.isTriggerWhenRelease)
        {
            Trigger();
            return;
        }
        OnRelease();
    }


    public IEnumerator ReleaseObject(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (args.Data.isTriggerWhenRelease)
        {
            Trigger();
            yield break;
        }
        OnRelease();
    }

    public void OnSpawn()
    {
        throw new NotImplementedException();
    }

    public void OnRelease()
    {
        if (ETFX != null)
        {
            ETFX.HitImpact();
        }
        PoolManager.Release(gameObject);
    }


    public void CreateAreaAttack()
    {

        UnityEngine.Assertions.Assert.IsNotNull(args.Data.collideData, "Warning: attackData cannot be null");
        UnityEngine.Assertions.Assert.IsNotNull(args.Data.collideData.prefab, "Warning: genericAttackPrefab cannot be null");
        GameObject attack = PoolManager.Spawn(args.Data.collideData.prefab, transform.position, Quaternion.identity);
        AreaAttack areaAttackComponent = attack.GetComponent<AreaAttack>();
        areaAttackComponent.ApplyAttackInfo(args.Data.collideData);
        areaAttackComponent.SetDamageInfo(args.damageInfo);
    }
}
