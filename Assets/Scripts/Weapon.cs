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

    private GameObject SpawnBullet()
    {
        // GameObject firedBullet = Instantiate(bullet, weaponTip.position, weaponTip.rotation);
        GameObject firedBullet = bulletPool.GetBullet(weaponTip);

        return firedBullet;
    }

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
