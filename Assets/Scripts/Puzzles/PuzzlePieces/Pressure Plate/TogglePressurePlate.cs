using UnityEngine;

public class TogglePressurePlate : BasePressurePlate
{
    public override bool StartInteract(Transform interactor)
    {
        SetActivated(!IsActivated); // toggles state

        if (isActivated)
            ActivateButton();
        else
            DeactivateButton();

        return isActivated;
    }

    public override void EndInteract()
    {
        // do nothing - stays in current state
    }
}
