using UnityEngine;

public class TogglePressurePlate : BasePressurePlate
{
    public override void StartInteract(Transform interactor)
    {
        SetActivated(!IsActivated); // toggles state

        if (isActivated)
            ActivateButton();
        else
            DeactivateButton();
    }

    public override void EndInteract()
    {
        // do nothing - stays in current state
    }
}
