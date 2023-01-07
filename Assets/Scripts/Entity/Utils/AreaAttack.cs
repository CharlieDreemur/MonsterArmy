using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public enum AttackShape { Circle, Box, Sector }

public class AreaAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public string attackName;
    [SerializeField, Min(0.0f)]
    private float attackDuration = 1.0f;
    [SerializeField, Min(0.0f)]
    private float totalDuration = 1.0f;
    [SerializeField, Min(0.0f)]
    private float delay = 0.0f;

    public int maxHitCount = -1;
    public int damageOverTimeHitCount = -1;
    public bool followInstigator = false;

    public AudioSource sfx;
    public AudioSource hitSound;

    private float damageOverTimeInterval = Mathf.Infinity;
    private float nextDamageTimestamp = Mathf.Infinity;

    [Header("Collider")]
    [SerializeField]
    private AttackShape colliderShape = AttackShape.Circle;
    public CapsuleCollider2D cachedCapsuleCollider;
    public BoxCollider2D cachedBoxCollider;
    private Collider2D hitbox;

    [Header("Circle Settings")]
    public float radius = 5.0f;
    [Header("Sector Settings")]
    [Range(0.0f, 360.0f)]
    public float sectorAngle = 45.0f;

    [Header("Box Settings")]
    [SerializeField]
    private Vector2 size = new(1.0f, 1.0f);

    public GameObject effect = null;
    public float effectScale = 1.0f;

    public UnityEvent onHit;
    public bool hitboxActive => hitbox.enabled;

    private DamageInfo damageInfo;
    private float timeElapsed = 0.0f;
    private HashSet<Entity> hitEntities;

    private int hitCount;
    private bool attackInitialized;
    private bool attackCanceled = false;

    void Awake()
    {
        hitEntities = new HashSet<Entity>();
        OnDisable();
    }

    void OnEnable()
    {
        hitEntities.Clear();
        timeElapsed = 0.0f;
        hitCount = 0;
        attackInitialized = false;
        attackCanceled = false;
    }

    void OnDisable()
    {
        if (cachedCapsuleCollider != null)
        {
            //cachedCapsuleCollider.height = 5.0f;
            cachedCapsuleCollider.enabled = false;
        }
        if (cachedBoxCollider != null)
        {
            cachedBoxCollider.enabled = false;
        }
    }

    //startup delay shouldn't be here, but oh well ¯\_(ツ)_/¯
    public void ApplyAttackInfo(CollideData data, float startupDelay = 0.0f)
    {
        if (data is null)
        {
            Debug.LogError("AttackData is null!");
        }

        followInstigator = data.followInstigator;
        attackDuration = data.attackDuration;
        totalDuration = data.totalDuration;
        colliderShape = data.attackShape;
        maxHitCount = data.maxHitCount;
        delay = startupDelay;
        effect = data.effect;
        effectScale = data.effectScale;
        if (sfx != null)
        {
            sfx.clip = data.sfx;
        }
        if (hitSound != null)
        {
            hitSound.clip = data.hitSound;
        }

        damageOverTimeHitCount = data.damageOverTimeHitCount;

        damageOverTimeInterval = Mathf.Infinity;
        nextDamageTimestamp = Mathf.Infinity;

        if (damageOverTimeHitCount > 0)
        {
            damageOverTimeInterval = attackDuration / damageOverTimeHitCount;
            nextDamageTimestamp = 0.0f;
        }
        else
        {
            damageOverTimeInterval = Mathf.Infinity;
        }

        if (colliderShape == AttackShape.Sector || colliderShape == AttackShape.Circle)
        {
            if (cachedCapsuleCollider)
            {
                hitbox = cachedCapsuleCollider;
            }
            else
            {
                Debug.LogError("AreaAttack has no cached Capsule Collider");
            }
            radius = data.radius;
            (hitbox as CapsuleCollider2D).size = new Vector2(radius, radius);
            sectorAngle = data.sectorAngle;
        }
        else
        {
            if (cachedBoxCollider)
            {
                hitbox = cachedBoxCollider;
            }
            else
            {
                Debug.LogError("AreaAttack has no cached Box Collider");
            }
            Vector2 extents = data.boxSize;
            (hitbox as BoxCollider2D).size = new Vector3(extents.x, 2.0f, extents.y);
            size = extents;
        }

        hitbox.isTrigger = true;
        hitbox.enabled = (delay <= 0.0f);
        attackInitialized = true;
    }

    public void SetDamageInfo(DamageInfo damageInfoIn, Entity instigator = null)
    {
        damageInfo = damageInfoIn;
        if (instigator)
        {
            damageInfo.attacker = instigator;
        }
    }

    void Update()
    {
        if (!attackInitialized)
        {
            Debug.LogError("AreaAttack parameters not initialized after being spawned!");
        }

        float previousTimeElapsed = timeElapsed;
        timeElapsed += Time.deltaTime;

        if (timeElapsed > attackDuration + delay)
        {
            hitbox.enabled = false;
            if (timeElapsed > totalDuration)
            {
                PoolManager.Release(gameObject);
            }
        }

        if (timeElapsed > nextDamageTimestamp)
        {
            nextDamageTimestamp += damageOverTimeInterval;
            if (hitbox.enabled)
            {
                hitEntities.Clear();
                if (effect != null)
                {
                    if (sfx != null)
                    {
                        sfx.pitch = Random.Range(0.85f, 1.1f);
                        sfx.Play();
                    }
                    var fx = Instantiate(effect, transform);
                    fx.transform.localScale = Vector3.one * effectScale;
                    fx.transform.position += Vector3.down * 0.5f;
                    Destroy(fx, 2.0f);
                }
            }
        }

        if (timeElapsed > delay && previousTimeElapsed <= delay)
        {
            hitbox.enabled = true;
            if (effect != null)
            {
                if (sfx != null)
                {
                    sfx.pitch = Random.Range(0.85f, 1.1f);
                    sfx.Play();
                }
                var fx = Instantiate(effect, transform);
                fx.transform.localScale = Vector3.one * effectScale;
                fx.transform.position += Vector3.down * 0.5f;
                Destroy(fx, 2.0f);
            }
        }

        if (attackCanceled) { hitbox.enabled = false; }

        if (followInstigator && damageInfo.attacker)
        {
            transform.position = damageInfo.attacker.gameObject.transform.position;
        }
    }

    public void CancelAttack()
    {
        attackCanceled = true;
    }

    Vector3 CalculateKnockbackDirection(GameObject gameObject, AttackShape attackShape)
    {
        if (attackShape == AttackShape.Circle || attackShape == AttackShape.Sector)
        {
            Vector3 offset = (gameObject.transform.position - transform.position);
            offset.y = 0.0f;
            return offset.normalized;
        }
        else
        {
            Vector3 forward = transform.forward;
            forward.y = 0.0f;
            return forward.normalized;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (timeElapsed > attackDuration + delay)
        {
            Debug.LogError("This shouldn't happen");
            return;
        }

        //TODO:Avoid Repetition Detection
        if (!hitboxActive) { return; }

        GameObject colliderObject = collider.gameObject;

        if (!colliderObject.TryGetComponent(out Entity entity)) { return; }

        if (entity == damageInfo.attacker) { return; }

        if (colliderShape == AttackShape.Sector)
        {
            if (hitEntities.Contains(entity) || !IsCollidedWithSector(sectorAngle, collider))
            {
                return;
            }
        }

        bool isFirstHit = hitEntities.Add(entity);
        if (!isFirstHit) { return; }
        Vector3 knockbackDirection = CalculateKnockbackDirection(colliderObject, colliderShape);
        (entity as IDamageable).SetKnockbackDirection(knockbackDirection);
        (entity as IDamageable).ApplyDamage(damageInfo);

        if (hitCount == 0 && hitSound != null)
        {
            hitSound.pitch = Random.Range(0.85f, 1.1f);
            hitSound.Play();
        }
        hitCount += 1;
        onHit.Invoke();
        if (maxHitCount > 0 && hitCount >= maxHitCount)
        {
            hitbox.enabled = false;
            return;
        }

    }

    public bool IsCollidedWithSector(float sectorAngle, Collider2D hitCollider)
    {
        float halfSectorAngle = sectorAngle * 0.5f;

        Vector2 sectorDir1 = transform.forward.RotateAboutUp(-halfSectorAngle);
        Vector2 sectorDir2 = transform.forward.RotateAboutUp(halfSectorAngle);

        Vector2 directionToCollider = hitCollider.transform.position - transform.position;
        //directionToCollider.y = 0.0f;
        float angleBetween = Vector2.Angle(transform.forward, directionToCollider);

        if (angleBetween < halfSectorAngle)
        {
            return true;
        }

        Ray2D ray1 = new(origin: transform.position, direction: sectorDir1);
        Ray2D ray2 = new(origin: transform.position, direction: sectorDir2);

        return hitCollider.OverlapPoint(ray1.GetPoint(radius))|| hitCollider.OverlapPoint(ray2.GetPoint(radius));
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!hitboxActive)
        {
            return;
        }

        Gizmos.color = Color.green;
        Handles.color = Color.green;
        switch (colliderShape)
        {
            case AttackShape.Circle:
                {
                    CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
                    if (collider == null) return;
                    Vector3 position = collider.transform.position;
                    float radius = collider.size.x * 0.5f;
                    float height = collider.size.y - radius * 2.0f;
                    Vector3 top = position + Vector3.up * height * 0.5f;
                    Vector3 bottom = position - Vector3.up * height * 0.5f;
                    Gizmos.DrawWireSphere(top, radius);
                    Gizmos.DrawWireSphere(bottom, radius);
                    Gizmos.DrawLine(top + Vector3.left * radius, bottom + Vector3.left * radius);
                    Gizmos.DrawLine(top + Vector3.right * radius, bottom + Vector3.right * radius);
                    break;
                }
            case AttackShape.Box:
                Vector3 boxOffset = new(0.0f, 0.0f, size.y * 0.5f);
                Vector3 boxSize = new(size.x, 0.5f, size.y);

                Handles.matrix = transform.localToWorldMatrix;

                Handles.DrawWireCube(boxOffset, boxSize);

                Handles.matrix = Matrix4x4.identity;
                break;
            case AttackShape.Sector:
                Handles.DrawWireDisc(transform.position, Vector3.up, radius, 1.0f);

                Vector3 sectorDir1 = transform.forward.RotateAboutUp(-sectorAngle * 0.5f);
                Vector3 sectorDir2 = transform.forward.RotateAboutUp(sectorAngle * 0.5f);

                Handles.color = Color.red;

                Handles.DrawLine(transform.position, transform.position + (radius * sectorDir1), 4.0f);
                Handles.DrawLine(transform.position, transform.position + (radius * sectorDir2), 4.0f);

                Handles.DrawWireArc(transform.position, Vector3.up, sectorDir1, sectorAngle, radius, 4.0f);
                break;

        }

    }
#endif
}