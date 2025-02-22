using UnityEngine;

public class DoubleDoor : BaseDoor
{
    [SerializeField]
    Transform doorHalfB;
    private Vector3 doorHalfBStartPos;

    protected override void Start()
    {
        base.Start();
        doorHalfBStartPos = doorHalfB.localPosition;
    }

    public override void OpenDoor()
    {
        // move door A
        MoveObject(movingObject, startPos, endTarget.localPosition, moveProgress, movementCurve);

        // move door B in opposite direction
        Vector3 doorBStart = doorHalfBStartPos;
        Vector3 doorBTarget = doorHalfBStartPos - (endTarget.localPosition - startPos);
        MoveObject(doorHalfB, doorBStart, doorBTarget, moveProgress, movementCurve);
    }

    public override void CloseDoor()
    {
        // move door A
        MoveObject(movingObject, endTarget.localPosition, startPos, moveProgress, movementCurve);

        // move door B in opposite direction
        Vector3 doorBStart = doorHalfBStartPos - (endTarget.localPosition - startPos);
        Vector3 doorBTarget = doorHalfBStartPos;
        MoveObject(doorHalfB, doorBStart, doorBTarget, moveProgress, movementCurve);
    }
}
