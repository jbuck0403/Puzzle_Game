using Unity.VisualScripting;
using UnityEngine;

public class SingleDoor : BaseDoor
{
    private float originalX;
    private float originalZ;

    protected override void Start()
    {
        base.Start();
        // store the original X and Z positions
        originalX = movingObject.localPosition.x;
        originalZ = movingObject.localPosition.z;
    }

    public override void OpenDoor()
    {
        Vector3 fromPos = new Vector3(originalX, startPos.y, originalZ);
        Vector3 toPos = new Vector3(originalX, endTarget.position.y, originalZ);
        MoveObject(movingObject, fromPos, toPos, moveProgress, movementCurve);
    }

    public override void CloseDoor()
    {
        Vector3 fromPos = new Vector3(originalX, endTarget.position.y, originalZ);
        Vector3 toPos = new Vector3(originalX, startPos.y, originalZ);
        MoveObject(movingObject, fromPos, toPos, moveProgress, movementCurve);
    }
}
