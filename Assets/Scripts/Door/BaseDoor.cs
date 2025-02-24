using UnityEditor.MPE;
using UnityEngine;

public abstract class BaseDoor : BaseMovable, IInteractable
{
    [SerializeField]
    protected bool isLocked = true;

    [SerializeField]
    private bool interactable = true;

    public bool IsLocked => isLocked;
    public bool IsClosed => IsAtStart;

    public bool CanInteract { get; protected set; }
    public float InteractRange { get; } = InteractDistance.Medium;

    public string PromptText => "Press E";

    protected virtual void Awake()
    {
        CanInteract = interactable;
    }

    public virtual void TriggerOpenDoor()
    {
        SetMovementState(MovementState.MovingToEnd);
    }

    public virtual void TriggerCloseDoor()
    {
        SetMovementState(MovementState.MovingToStart);
    }

    public bool StartInteract(Transform interactor)
    {
        if (interactor != null)
        {
            KeyHandler keyHandler = interactor.GetComponent<KeyHandler>();
            if (keyHandler != null)
            {
                UnlockDoor(keyHandler.UseKey());
            }
        }

        if (!IsLocked && !isMovingToEnd && !isMovingToStart)
        {
            if (IsClosed)
            {
                print("OPENING");
                TriggerOpenDoor();
            }
            else
            {
                print("CLOSING");
                TriggerCloseDoor();
            }
        }

        return isMovingToEnd || isMovingToStart;
    }

    public void EndInteract() { }

    protected void RotateDoor() { }

    public void UnlockDoor(bool condition)
    {
        if (condition)
        {
            isLocked = false;
        }
    }

    public void LockDoor(bool condition)
    {
        if (condition)
        {
            isLocked = true;
        }
    }

    public virtual void ForceOpenDoor()
    {
        UnlockDoor(true);
        TriggerOpenDoor();
    }

    public virtual void ForceCloseDoor()
    {
        LockDoor(true);
        TriggerCloseDoor();
    }

    public override void OnPuzzleCompleted()
    {
        ForceOpenDoor();
    }

    public override void OnPuzzleUnsolved()
    {
        ForceCloseDoor();
    }

    public abstract void OpenDoor();

    public abstract void CloseDoor();

    // Implement BaseMovable abstract methods
    public override void MoveToEnd()
    {
        OpenDoor();
    }

    public override void MoveToStart()
    {
        CloseDoor();
    }
}
