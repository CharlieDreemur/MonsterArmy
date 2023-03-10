using UnityEngine;
using System.Collections;


public class ETFXProjectile : MonoBehaviour
{
    public UnityEngine.GameObject impactParticle; // Effect spawned when projectile hits a collider
    public UnityEngine.GameObject projectileParticle; // Effect attached to the gameobject as child
    public UnityEngine.GameObject muzzleParticle; // Effect instantly spawned when gameobject is spawned
    public Vector3 scale = Vector3.one;
    public UnityEngine.GameObject projectileP;
    public RaycastHit hit;
    public void Init()
    {

        projectileP = PoolManager.Spawn(projectileParticle, transform.position, transform.rotation) as UnityEngine.GameObject;
        projectileP.transform.parent = transform;
        projectileP.transform.localScale = scale;
        if (muzzleParticle)
        {
            UnityEngine.GameObject muzzleP = PoolManager.Spawn(muzzleParticle, transform.position, transform.rotation) as UnityEngine.GameObject;
            muzzleP.transform.localScale = scale;
            muzzleP.transform.rotation = transform.rotation;
            StartCoroutine(ReleaseTimer(muzzleP, 1.5f)); // 2nd parameter is lifetime of effect in seconds
        }
    }

    public void SetScale(Vector3 scale)
    {
        this.scale = scale;
    }
    public void HitImpact()
    {
        UnityEngine.GameObject impactP = PoolManager.Spawn(impactParticle, transform.position, Quaternion.identity) as UnityEngine.GameObject; // Spawns impact effect
        impactP.transform.localScale = scale;
        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>(); // Gets a list of particle systems, as we need to detach the trails
                                                                             //Component at [0] is that of the parent i.e. this object (if there is any)
        for (int i = 1; i < trails.Length; i++) // Loop to cycle through found particle systems
        {
            ParticleSystem trail = trails[i];
            if (trail.gameObject.name.Contains("Trail"))
            {
                trail.transform.SetParent(null); // Detaches the trail from the projectile
                StartCoroutine(ReleaseTimer(trail.gameObject, 2f)); // Removes the trail after seconds
            }
        }
        StartCoroutine(ReleaseTimer(projectileP, 3f)); // Removes particle effect after delay
        StartCoroutine(ReleaseTimer(impactP, 3.5f)); // Removes impact effect after delay
        ///Destroy(gameObject); // Removes the projectile
    }

    IEnumerator ReleaseTimer(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PoolManager.Release(obj);
    }

}