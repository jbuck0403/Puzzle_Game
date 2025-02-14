using UnityEngine;

public class BulletPool : PoolBase
{
    [SerializeField]
    int poolCount;

    void Update()
    {
        poolCount = pool.Count;

        if (
            initialized
            && poolCount <= numObjects / 2
            && currentNumObjects < maxPoolSize - (incrementSize * 2)
        )
        {
            if (poolCount <= numObjects / incrementSize * 2)
            {
                IncreasePoolSize(incrementSize * 2);
            }
            else
            {
                IncreasePoolSize(incrementSize);
            }
        }
    }

    private void RepositionBulletForFiring(GameObject obj, Transform weaponTip)
    {
        obj.transform.SetPositionAndRotation(weaponTip.position, weaponTip.rotation);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public GameObject GetBullet(Transform weaponTip)
    {
        GameObject bullet = GetObject();
        RepositionBulletForFiring(bullet, weaponTip);

        return bullet;
    }

    protected override GameObject AddObjectToPool()
    {
        GameObject obj = base.AddObjectToPool();

        if (obj != null)
        {
            Projectile projectile = obj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Initialize(this);
            }
        }

        return obj;
    }
}
