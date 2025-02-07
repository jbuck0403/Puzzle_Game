using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField]
    public Transform weaponTip;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletSpeed = 500f;
    private Collectible weaponCollectible;
    BulletPool bulletPool;

    private GameObject SpawnBullet()
    {
        // GameObject firedBullet = Instantiate(bullet, weaponTip.position, weaponTip.rotation);
        GameObject firedBullet = bulletPool.GetObject();

        return firedBullet;
    }

    void Start()
    {
        weaponCollectible = GetComponent<Collectible>();
        GameObject bulletPoolObj = GameObject.FindGameObjectWithTag("BulletPool");
        bulletPool = bulletPoolObj.GetComponent<BulletPool>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && weaponCollectible.CheckCollected())
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        if (weaponCollectible.CheckCollected())
        {
            GameObject firedBullet = SpawnBullet();
            Rigidbody rb = firedBullet.GetComponent<Rigidbody>();

            Vector3 bulletForce = weaponTip.forward * bulletSpeed;

            rb.AddForce(bulletForce);
        }
    }
}
