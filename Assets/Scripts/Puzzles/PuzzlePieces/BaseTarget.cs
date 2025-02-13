using UnityEngine;

public class BaseTarget : TogglePressurePlate
{
    [SerializeField]
    bool destroyProjectile = true;

    public override bool StartInteract(Transform interactor)
    {
        bool result = base.StartInteract(interactor);

        if (destroyProjectile && interactor != null)
        {
            Projectile projectile = interactor.GetComponent<Projectile>();
            if (projectile != null)
                projectile.DestroyProjectile();
        }

        EndInteract();

        return result;
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartInteract(collision.transform);
    }
}
