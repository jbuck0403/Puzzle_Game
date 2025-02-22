using System;
using UnityEngine;

// base class that provides common puzzle piece functionality
public abstract class BasePuzzlePiece : MonoBehaviour
{
    [SerializeField]
    protected LayerMask interactibleLayer;

    public event Action OnStateChanged;

    protected PuzzleEvent onStateChangedEvent;

    [SerializeField]
    protected bool isActivated = false;

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
        if (puzzle != null && puzzle.IsCompleted && !puzzle.canBeUnsolved)
            return;

        if (isActivated != activated)
        {
            isActivated = activated;
            if (onStateChangedEvent != null)
            {
                onStateChangedEvent.RaiseSolvedEvent();
            }
            else
            {
                OnStateChanged?.Invoke();
            }
        }
    }
}
