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

        RecheckStandingObjects();
    }

    protected void RecheckStandingObjects()
    {
        // check for objects already inside the trigger
        Collider[] hitColliders = Physics.OverlapBox(
            transform.TransformPoint(interactZone.center),
            interactZone.size * 0.5f,
            transform.rotation,
            interactibleLayer
        );

        // add any valid colliders to our standing list
        foreach (Collider col in hitColliders)
        {
            if (col != null && col.GetComponent<Projectile>() == null)
            {
                standingOnPlate.Add(col);
            }
        }
    }

    protected virtual bool InteractConditions()
    {
        return numStandingBefore == 0;
    }

    protected virtual bool EndInteractConditions()
    {
        return standingOnPlate.Count == 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        // check if object is on the correct layer
        if ((interactibleLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (other.GetComponent<Projectile>() == null)
        {
            numStandingBefore = standingOnPlate.Count;
            standingOnPlate.Add(other);
        }

        if (InteractConditions())
        {
            StartInteract(other.transform);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other == null)
            return;

        // only process if it was a valid object
        if ((interactibleLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (other.GetComponent<Projectile>() == null)
        {
            standingOnPlate.Remove(other);
        }

        // only deactivate if nothing is left on the plate
        if (EndInteractConditions())
        {
            EndInteract();
        }
    }

    public override void ForceDeactivate()
    {
        standingOnPlate.Clear();
        base.ForceDeactivate();

        // Recheck for objects still physically on the plate
        RecheckStandingObjects();

        // If objects are still present, handle according to the pressure plate type
        if (standingOnPlate.Count > 0)
        {
            OnObjectsStillPresent();
        }
    }

    protected virtual void OnObjectsStillPresent()
    {
        // Base implementation does nothing
        // Derived classes can override this to handle objects still being present after force deactivate
    }

    // protected override void OnDisable()
    // {
    //     standingOnPlate.Clear();
    //     base.OnDisable();
    // }
}
