using UnityEngine;

public class Weapon : Pickupable
{
    [SerializeField]
    public Transform weaponTip;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletSpeed = 50f;
    BulletPool bulletPool;

    protected override void Start()
    {
        base.Start();

        GameObject bulletPoolObj = GameObject.FindGameObjectWithTag("BulletPool");
        bulletPool = bulletPoolObj.GetComponent<BulletPool>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && isCollected)
        {
            print("firing");
            ShootBullet();
        }
    }

    private GameObject SpawnBullet()
    {
        return bulletPool.GetBullet(weaponTip);
    }

    void ShootBullet()
    {
        if (isCollected)
        {
            GameObject firedBullet = SpawnBullet();
            Rigidbody rb = firedBullet.GetComponent<Rigidbody>();

            Vector3 bulletForce = weaponTip.forward * bulletSpeed;

            rb.AddForce(bulletForce);
        }
    }
}
