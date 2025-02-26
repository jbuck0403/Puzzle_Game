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

    protected void RecheckStandingObjects(bool interact = true)
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
                CheckStandingOnPlate(col);

                if (interact)
                    StartInteractWithConditions(col.transform);
            }
        }
    }

    protected void StartInteractWithConditions(Transform transform)
    {
        // check if this addition should trigger the pressure plate
        if (InteractConditions())
        {
            StartInteract(transform);
        }
    }

    protected void EndInteractWithConditions()
    {
        // check if this addition should trigger the pressure plate
        if (EndInteractConditions())
        {
            EndInteract();
        }
    }

    private void CheckStandingOnPlate(Collider col)
    {
        numStandingBefore = standingOnPlate.Count;
        standingOnPlate.Add(col);
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
            CheckStandingOnPlate(other);
        }
        StartInteractWithConditions(other.transform);
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

        EndInteractWithConditions();
    }

    public override void ForceDeactivate()
    {
        standingOnPlate.Clear();
        base.ForceDeactivate();

        // Recheck for objects still physically on the plate
        RecheckStandingObjects(false);

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
}
