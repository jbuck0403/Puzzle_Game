using System.Collections.Generic;
using UnityEngine;

public class BasePressurePlate : BaseButton
{
    protected HashSet<Collider> standingOnPlate = new HashSet<Collider>();
    protected int numStandingBefore;

    protected override void Start()
    {
        base.Start();

        CanInteract = false; // disallow IInteractable behavior
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        // check if object is on the correct layer
        if ((interactibleLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        numStandingBefore = standingOnPlate.Count;
        standingOnPlate.Add(other);

        StartInteract(other.transform);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other == null)
            return;

        // only process if it was a valid object
        if ((interactibleLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        standingOnPlate.Remove(other);

        // only deactivate if nothing is left on the plate
        if (standingOnPlate.Count == 0)
        {
            EndInteract();
        }
    }

    public override void ForceDeactivate()
    {
        standingOnPlate.Clear();
        base.ForceDeactivate();
    }

    // protected override void OnDisable()
    // {
    //     standingOnPlate.Clear();
    //     base.OnDisable();
    // }
}
