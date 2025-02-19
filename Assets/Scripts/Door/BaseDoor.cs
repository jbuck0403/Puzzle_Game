using UnityEditor.MPE;
using UnityEngine;

public abstract class BaseDoor : MonoBehaviour, IInteractable
{
    protected enum DoorState
    {
        Open,
        Closed,
        Opening,
        Closing
    }

    protected DoorState currentState = DoorState.Closed;

    [SerializeField]
    protected bool isLocked = true;

    [SerializeField]
    private bool openOnStart = false;

    [SerializeField]
    private bool interactable = true;

    [SerializeField]
    private BaseEvent OnPuzzleEvent;

    private bool isClosed = true;
    protected bool beingOpened = false;
    protected bool beingClosed = false;
    public bool IsLocked => isLocked;
    public bool IsClosed => isClosed;

    public bool CanInteract { get; protected set; }
    public float InteractRange { get; } = InteractDistance.Medium;

    [SerializeField]
    protected float openSpeed = 1f;

    [SerializeField]
    protected AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected float moveProgress = 0f;

    [SerializeField]
    protected Transform door;

    [SerializeField]
    protected Transform doorTarget;

    protected Vector3 doorStartPos;

    private bool lockedByDefault;

    protected virtual void Awake()
    {
        CanInteract = interactable;
        lockedByDefault = isLocked;
    }

    protected virtual void OnEnable()
    {
        if (OnPuzzleEvent != null)
        {
            if (OnPuzzleEvent is PuzzleEvent @puzzleEvent)
            {
                @puzzleEvent.Subscribe(OnPuzzleCompleted, OnPuzzleUnsolved);
            }
        }
    }

    protected virtual void Start()
    {
        doorStartPos = door.localPosition;

        if (openOnStart)
        {
            beingOpened = true;
            isClosed = false;
        }
    }

    public void TriggerOpenDoor()
    {
        SetDoorState(DoorState.Opening);
    }

    public void TriggerCloseDoor()
    {
        SetDoorState(DoorState.Closing);
    }

    void Update()
    {
        if (currentState == DoorState.Opening)
        {
            OpenDoor();
            SetOpenOrClosed(DoorState.Opening);
        }
        else if (currentState == DoorState.Closing)
        {
            CloseDoor();
            SetOpenOrClosed(DoorState.Closing);
        }
    }

    private void SetOpenOrClosed(DoorState currentState)
    {
        if (moveProgress >= 1f)
        {
            SetDoorState(currentState == DoorState.Opening ? DoorState.Open : DoorState.Closed);
            moveProgress = 0f;
        }
    }

    protected virtual void OnDisable()
    {
        if (OnPuzzleEvent != null)
        {
            if (OnPuzzleEvent is PuzzleEvent @event)
            {
                @event.Unsubscribe(OnPuzzleCompleted, OnPuzzleUnsolved);
            }
        }
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

        if (!IsLocked && !beingOpened && !beingClosed)
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

        return beingOpened || beingClosed;
    }

    public void EndInteract() { }

    protected void RotateDoor() { }

    protected void SlideDoor(
        Transform door,
        Vector3 startPos,
        Vector3 targetPos,
        float progress,
        AnimationCurve curve
    )
    {
        Vector3 newPos = Vector3.Lerp(startPos, targetPos, curve.Evaluate(progress));
        door.localPosition = newPos;
    }

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

    public void ForceOpenDoor()
    {
        UnlockDoor(true);
        TriggerOpenDoor();
    }

    public void ForceCloseDoor()
    {
        LockDoor(true);
        TriggerCloseDoor();
    }

    private void OnPuzzleCompleted()
    {
        ForceOpenDoor();
    }

    private void OnPuzzleUnsolved()
    {
        ForceCloseDoor();
    }

    public abstract void OpenDoor();

    public abstract void CloseDoor();

    protected void SetDoorState(DoorState newState)
    {
        // handle animation progress when changing between Opening/Closing
        if (
            (currentState == DoorState.Opening && newState == DoorState.Closing)
            || (currentState == DoorState.Closing && newState == DoorState.Opening)
        )
        {
            moveProgress = 1 - moveProgress;
        }

        // update state
        currentState = newState;

        // update helper flags based on state
        beingOpened = newState == DoorState.Opening;
        beingClosed = newState == DoorState.Closing;
        isClosed = newState == DoorState.Closed;

        Debug.Log($"Door state changed to: {newState}");
    }
}
