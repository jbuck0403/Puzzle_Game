using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedPressurePlate : BasePressurePlate
{
    protected override void Start()
    {
        base.Start();
        base.ActivateButton();
    }

    protected override void ActivateButton()
    {
        base.DeactivateButton();
    }

    protected override void DeactivateButton()
    {
        base.ActivateButton();
    }
}
