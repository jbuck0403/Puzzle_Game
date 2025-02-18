using UnityEngine;

public abstract class BaseDoor : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected bool isLocked = true;

    [SerializeField]
    private bool openOnStart = false;

    [SerializeField]
    private bool interactable = true;

    [SerializeField]
    private BaseEvent OnPuzzleComplete;

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
    protected AnimationCurve openMovementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField]
    protected AnimationCurve closeMovementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // if using with standalone pressure plate, ensure this matches openMovementCurve to avoid broken animations

    protected AnimationCurve movementCurve;

    protected float moveProgress = 0f;

    [SerializeField]
    protected Transform door;

    [SerializeField]
    protected Transform doorTarget;

    protected Vector3 doorStartPos;

    protected virtual void Awake()
    {
        CanInteract = interactable;
    }

    protected virtual void OnEnable()
    {
        if (OnPuzzleComplete != null)
        {
            if (OnPuzzleComplete is PuzzleEvent @puzzleEvent)
            {
                @puzzleEvent.Subscribe(OnPuzzleCompleted);
            }
            else if (OnPuzzleComplete is DoorEvent @doorEvent)
            {
                @doorEvent.SubscribeToOpen(TriggerOpenDoor);
                @doorEvent.SubscribeToClose(TriggerCloseDoor);
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
        if (beingClosed) // If door is currently opening
        {
            moveProgress = 1 - moveProgress;
            beingOpened = false;
        }
        movementCurve = openMovementCurve;
        beingOpened = true;
        isClosed = false;
    }

    public void TriggerCloseDoor()
    {
        if (beingOpened) // If door is currently opening
        {
            moveProgress = 1 - moveProgress;
            beingOpened = false;
        }
        movementCurve = closeMovementCurve;
        beingClosed = true;
        isClosed = true;
    }

    void Update()
    {
        if (beingOpened)
        {
            OpenDoor();
        }
        else if (beingClosed)
        {
            CloseDoor();
        }
    }

    protected virtual void OnDisable()
    {
        if (OnPuzzleComplete != null)
        {
            if (OnPuzzleComplete is PuzzleEvent @event)
            {
                @event.Unsubscribe(OnPuzzleCompleted);
            }
            else if (OnPuzzleComplete is DoorEvent @doorEvent)
            {
                @doorEvent.UnsubscribeFromOpen(TriggerOpenDoor);
                @doorEvent.UnsubscribeFromClose(TriggerCloseDoor);
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
        StartInteract(default);
        // if (IsClosed && !beingOpened)
        // {
        //     beingOpened = true;
        //     isClosed = false;
        // }
    }

    private void OnPuzzleCompleted()
    {
        UnlockDoor(true);
        ForceOpenDoor();
    }

    public abstract void OpenDoor();

    public abstract void CloseDoor();
}
