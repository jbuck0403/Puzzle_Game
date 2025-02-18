using System;
using UnityEngine;

// base class that provides common puzzle piece functionality
public abstract class BasePuzzlePiece : MonoBehaviour
{
    [SerializeField]
    protected LayerMask interactibleLayer;

    public event Action OnStateChanged;

    [SerializeField]
    protected PuzzleEvent onStateChangedEvent;

    [SerializeField]
    protected bool isActivated = false;

    [SerializeField]
    protected bool standalonePlate = false;

    public bool IsActivated => isActivated;

    protected BasePuzzle puzzle;

    protected virtual void Start()
    {
        puzzle = GetComponentInParent<BasePuzzle>();
        if (puzzle != null)
        {
            puzzle.AddPuzzlePiece(this);
        }
        else
        {
            Debug.LogWarning($"No puzzle found for puzzle piece {gameObject.name}");
            return;
        }
    }

    protected void SetActivated(bool activated)
    {
        // if in a puzzle, only block if puzzle is completed
        if (puzzle != null && puzzle.IsCompleted)
            return;

        // if not in a puzzle, require standalone and event
        if (puzzle == null && (!standalonePlate || onStateChangedEvent == null))
        {
            Debug.LogError(
                $"No Puzzle found && standalonePlate == {standalonePlate} || Event {(onStateChangedEvent != null ? "Present" : "Missing")}"
            );
            return;
        }

        if (isActivated != activated)
        {
            isActivated = activated;
            if (onStateChangedEvent != null && !standalonePlate)
            {
                onStateChangedEvent.RaiseEvent();
            }
            else
            {
                OnStateChanged?.Invoke();
            }
        }
    }
}
