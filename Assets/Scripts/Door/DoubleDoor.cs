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
        MoveDoors(doorStartPos, doorTarget.localPosition, ref beingOpened);
    }

    public override void CloseDoor()
    {
        MoveDoors(doorTarget.localPosition, doorStartPos, ref beingClosed);
    }

    private void MoveDoors(Vector3 doorAStart, Vector3 doorATarget, ref bool movementFlag)
    {
        if (moveProgress < 1f)
        {
            moveProgress += Time.deltaTime * openSpeed;

            Vector3 doorAMovement = doorATarget - doorAStart;

            SlideDoor(door, doorAStart, doorATarget, moveProgress, movementCurve);

            Vector3 doorBStart =
                (doorATarget == doorStartPos)
                    ? doorHalfBStartPos - (doorTarget.localPosition - doorStartPos)
                    : doorHalfBStartPos;

            SlideDoor(
                doorHalfB,
                doorBStart,
                doorBStart - doorAMovement,
                moveProgress,
                movementCurve
            );
        }
        else
        {
            movementFlag = false;
            moveProgress = 0f;
        }
    }
}
