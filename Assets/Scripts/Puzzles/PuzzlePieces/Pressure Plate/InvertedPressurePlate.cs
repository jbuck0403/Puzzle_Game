using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedPressurePlate : BasePressurePlate
{
    private bool firstActivation = true;

    protected override void Start()
    {
        base.Start();

        if (standingOnPlate.Count == 0)
        {
            base.ActivateButton();
        }
    }

    protected override void ActivateButton()
    {
        base.DeactivateButton();
    }

    protected override void DeactivateButton()
    {
        base.ActivateButton();
    }

    protected override void PlayActivateSound()
    {
        if (firstActivation)
        {
            firstActivation = false; // ensure pressure plate doesn't play a sound on initial activation
        }
        else
        {
            base.PlayActivateSound();
        }
    }
}
