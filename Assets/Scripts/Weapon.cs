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

    ProjectileAudioHandler audioHandler;

    protected override void Start()
    {
        base.Start();

        GameObject bulletPoolObj = GameObject.FindGameObjectWithTag("BulletPool");
        bulletPool = bulletPoolObj.GetComponent<BulletPool>();

        audioHandler = GetComponent<ProjectileAudioHandler>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && isCollected)
        {
            PlayWeaponSound();
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

    protected void PlayWeaponSound()
    {
        audioHandler.PlayActivateSound();
    }
}
