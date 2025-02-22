using Unity.VisualScripting;
using UnityEngine;

public class BasePlatform : BaseMovable
{
    [SerializeField]
    protected BoxCollider platformTrigger; // separate trigger collider for detecting the player

    protected Vector3 lastWorldPosition;

    protected override void Start()
    {
        base.Start();
        lastWorldPosition = movingObject.transform.position;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(movingObject);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    public override void MoveToEnd()
    {
        MoveObject(movingObject, startPos, endTarget.localPosition, moveProgress, movementCurve);
    }

    public override void MoveToStart()
    {
        MoveObject(movingObject, endTarget.localPosition, startPos, moveProgress, movementCurve);
    }
}
