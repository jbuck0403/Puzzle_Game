using UnityEngine;

public class ToggleTarget : BaseTarget
{
    public override bool StartInteract(Transform interactor)
    {
        bool result = base.StartInteract(interactor);

        DestroyProjectile(interactor);

        return result;
    }
}
