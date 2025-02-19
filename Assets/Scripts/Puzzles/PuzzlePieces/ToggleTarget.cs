using UnityEngine;

public class ToggleTarget : BaseTarget
{
    public override bool StartInteract(Transform interactor)
    {
        bool result = base.StartInteract(interactor);

        DestroyProjectile(interactor);

        EndInteract();

        return result;
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     StartInteract(collision.transform);
    // }
}
