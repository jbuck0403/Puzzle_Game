using Unity.VisualScripting;
using UnityEngine;

public class SingleDoor : BaseDoor
{
    public override void OpenDoor()
    {
        MoveDoor(doorStartPos, doorTarget.localPosition, ref beingOpened);
    }

    public override void CloseDoor()
    {
        MoveDoor(doorTarget.localPosition, doorStartPos, ref beingClosed);
    }

    private void MoveDoor(Vector3 startPos, Vector3 targetPos, ref bool movementFlag)
    {
        if (moveProgress < 1f)
        {
            moveProgress += Time.deltaTime * openSpeed;
            SlideDoor(door, startPos, targetPos, moveProgress, movementCurve);
        }
        else
        {
            movementFlag = false;
            moveProgress = 0f;
        }
    }
}
