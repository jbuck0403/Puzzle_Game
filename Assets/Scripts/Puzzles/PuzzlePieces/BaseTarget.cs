using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTarget : TogglePressurePlate
{
    [SerializeField]
    bool destroyProjectile = true;

    protected void DestroyProjectile(Transform interactor)
    {
        if (destroyProjectile && interactor != null)
        {
            Projectile projectile = interactor.GetComponent<Projectile>();
            if (projectile != null)
                projectile.DestroyProjectile();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartInteract(collision.transform);
    }
}
