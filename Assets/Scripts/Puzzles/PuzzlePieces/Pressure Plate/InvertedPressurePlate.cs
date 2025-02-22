using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedPressurePlate : BasePressurePlate
{
    protected override void Start()
    {
        base.Start();

        if (standingOnPlate.Count > 0)
        {
            DeactivateButton();
        }
        else
        {
            ActivateButton();
        }
    }

    public override bool StartInteract(Transform interactor)
    {
        if (!disabled)
        {
            DeactivateButton();
            return IsActivated;
        }
        return false;
    }

    public override void EndInteract()
    {
        if (!disabled)
        {
            ActivateButton();
        }
    }
}
