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
    PlayerInput playerInput;

    ProjectileAudioHandler audioHandler;

    protected override void Start()
    {
        base.Start();

        GameObject bulletPoolObj = GameObject.FindGameObjectWithTag("BulletPool");
        bulletPool = bulletPoolObj.GetComponent<BulletPool>();
        audioHandler = GetComponent<ProjectileAudioHandler>();

        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && isCollected)
        {
            ShootBullet();
        }
    }

    private GameObject SpawnBullet()
    {
        return bulletPool.GetBullet(weaponTip);
    }

    void ShootBullet()
    {
        if (isCollected && playerInput != null && !playerInput.IsPaused)
        {
            PlayWeaponSound();
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
