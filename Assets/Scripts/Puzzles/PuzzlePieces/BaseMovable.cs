using UnityEngine;

public abstract class BaseMovable : MonoBehaviour, IPuzzleAffected
{
    public enum MovementState
    {
        AtStart,
        AtEnd,
        MovingToEnd,
        MovingToStart
    }

    public MovementState currentState = MovementState.AtStart;

    [SerializeField]
    protected float moveSpeed = 1f;

    [SerializeField]
    protected AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public float moveProgress = 0f;

    [SerializeField]
    protected Transform movingObject;

    [SerializeField]
    protected Transform endTarget;

    [SerializeField]
    protected BaseEvent OnPuzzleEvent;

    protected Vector3 startPos;
    protected bool isMovingToEnd = false;
    protected bool isMovingToStart = false;
    public bool IsAtStart => currentState == MovementState.AtStart;

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

    protected virtual void OnDisable()
    {
        if (OnPuzzleEvent != null)
        {
            if (OnPuzzleEvent is PuzzleEvent @puzzleEvent)
            {
                @puzzleEvent.Unsubscribe(OnPuzzleCompleted, OnPuzzleUnsolved);
            }
        }
    }

    protected virtual void Start()
    {
        startPos = movingObject.localPosition;
    }

    protected virtual void Update()
    {
        if (currentState == MovementState.MovingToEnd)
        {
            UpdateMoveProgress();
            MoveToEnd();
            SetEndpointState(MovementState.MovingToEnd);
        }
        else if (currentState == MovementState.MovingToStart)
        {
            UpdateMoveProgress();
            MoveToStart();
            SetEndpointState(MovementState.MovingToStart);
        }

        HandleMovementComplete();
    }

    private void HandleMovementComplete()
    {
        if (moveProgress >= 1f)
        {
            moveProgress = 0f;
            if (currentState == MovementState.MovingToEnd)
            {
                SetMovementState(MovementState.AtEnd);
            }
            else if (currentState == MovementState.MovingToStart)
            {
                SetMovementState(MovementState.AtStart);
            }
        }
    }

    public void TriggerMoveToEnd()
    {
        SetMovementState(MovementState.MovingToEnd);
    }

    public void TriggerMoveToStart()
    {
        SetMovementState(MovementState.MovingToStart);
    }

    protected void SetEndpointState(MovementState currentState)
    {
        if (moveProgress >= 1f)
        {
            SetMovementState(
                currentState == MovementState.MovingToEnd
                    ? MovementState.AtEnd
                    : MovementState.AtStart
            );
            moveProgress = 0f;
        }
    }

    protected virtual void UpdateMoveProgress()
    {
        moveProgress += Time.deltaTime * moveSpeed;
    }

    protected void MoveObject(
        Transform objectToMove,
        Vector3 startPos,
        Vector3 targetPos,
        float progress,
        AnimationCurve curve
    )
    {
        Vector3 newPos = Vector3.Lerp(startPos, targetPos, curve.Evaluate(progress));
        objectToMove.localPosition = newPos;
    }

    protected void SetMovementState(MovementState newState)
    {
        // handle animation progress when changing between MovingToEnd/MovingToStart
        if (
            (currentState == MovementState.MovingToEnd && newState == MovementState.MovingToStart)
            || (
                currentState == MovementState.MovingToStart && newState == MovementState.MovingToEnd
            )
        )
        {
            moveProgress = 1 - moveProgress;
        }

        // update state
        currentState = newState;

        // update helper flags based on state
        isMovingToEnd = newState == MovementState.MovingToEnd;
        isMovingToStart = newState == MovementState.MovingToStart;
    }

    public abstract void MoveToEnd();
    public abstract void MoveToStart();

    // IPuzzleAffected implementation
    public virtual void OnPuzzleCompleted()
    {
        TriggerMoveToEnd();
    }

    public virtual void OnPuzzleUnsolved()
    {
        TriggerMoveToStart();
    }
}
