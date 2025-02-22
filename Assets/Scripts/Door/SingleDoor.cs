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
        originalX = door.localPosition.x;
        originalZ = door.localPosition.z;
    }

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
            // Only move on Y axis for single door, using original X and Z positions
            float newY = Mathf.Lerp(startPos.y, targetPos.y, movementCurve.Evaluate(moveProgress));
            Vector3 newPos = new Vector3(originalX, newY, originalZ);
            door.localPosition = newPos;
        }
        else
        {
            movementFlag = false;
            moveProgress = 0f;
        }
    }
}
