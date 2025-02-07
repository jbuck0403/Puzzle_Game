using UnityEngine;

public class BulletPool : PoolBase
{
    PlayerInput player;
    Transform weaponTip;

    [SerializeField]
    int poolCount;

    protected override void Start()
    {
        base.Start();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<PlayerInput>();

        FireProjectile fireProjectile = player.currentWeapon.GetComponent<FireProjectile>();
        weaponTip = fireProjectile.weaponTip;
    }

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

    protected override void OnObjectSpawned(GameObject obj)
    {
        RepositionBulletForFiring(obj);
    }

    private void RepositionBulletForFiring(GameObject obj)
    {
        obj.transform.position = weaponTip.position;
        obj.transform.rotation = weaponTip.rotation;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
