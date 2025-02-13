using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    float selfDestructTime = 0f;

    private PoolBase pool;

    void Start()
    {
        if (selfDestructTime != 0f)
            StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTime);

        DestroyProjectile();
    }

    public void Initialize(PoolBase bulletPool)
    {
        pool = bulletPool;
    }

    public void DestroyProjectile()
    {
        if (pool != null)
        {
            pool.ReturnToPool(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
