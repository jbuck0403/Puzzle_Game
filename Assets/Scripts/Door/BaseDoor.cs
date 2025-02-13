using UnityEngine;

public abstract class BaseDoor : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected bool isLocked = true;
    private bool isClosed = true;
    protected bool beingOpened = false;
    protected bool beingClosed = false;
    public bool IsLocked => isLocked;
    public bool IsClosed => isClosed;

    public bool CanInteract { get; private set; } = true;
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

    protected virtual void Start()
    {
        doorStartPos = door.localPosition;
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

    public bool StartInteract(Transform interactor)
    {
        KeyHandler keyHandler = interactor.GetComponent<KeyHandler>();
        UnlockDoor(keyHandler.UseKey());

        if (!IsLocked && !beingOpened && !beingClosed)
        {
            if (IsClosed)
            {
                beingOpened = true;
                isClosed = false;
            }
            else
            {
                beingClosed = true;
                isClosed = true;
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

    public abstract void OpenDoor();

    public abstract void CloseDoor();
}
