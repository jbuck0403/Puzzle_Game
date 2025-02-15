using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePuzzle : MonoBehaviour
{
    // subscribe to this event to trigger what happens when the puzzle is solved
    // public event Action OnPuzzleCompleted;

    public event Action OnPuzzleStateChanged;

    [SerializeField]
    protected PuzzleEvent OnPuzzleCompleted;

    protected List<BasePuzzlePiece> puzzlePieces = new List<BasePuzzlePiece>();

    protected bool isCompleted = false;

    public bool IsCompleted => isCompleted;

    private void Awake()
    {
        if (OnPuzzleCompleted != null)
        {
            OnPuzzleCompleted.Subscribe(() => ResetPuzzle(true));
        }
    }

    public virtual void AddPuzzlePiece(BasePuzzlePiece piece)
    {
        if (!puzzlePieces.Contains(piece))
        {
            puzzlePieces.Add(piece);
            // subscribe to piece state changes
            piece.OnStateChanged += OnPieceStateChanged;
        }
    }

    public virtual void RemovePuzzlePiece(BasePuzzlePiece piece)
    {
        if (puzzlePieces.Contains(piece))
        {
            // unsubscribe from piece state changes
            piece.OnStateChanged -= OnPieceStateChanged;
            puzzlePieces.Remove(piece);
        }
    }

    protected virtual void OnPieceStateChanged()
    {
        OnPuzzleStateChanged?.Invoke();
        CheckPuzzleSolved();
    }

    public virtual void CheckPuzzleSolved()
    {
        if (IsPuzzleConditionMet() && !isCompleted)
        {
            print("Puzzle Completed!");
            isCompleted = true;
            if (OnPuzzleCompleted != null)
            {
                OnPuzzleCompleted.RaiseEvent();
            }
        }
    }

    public virtual void ResetPuzzle(bool disable = false)
    {
        print($"RESETTING PUZZLE (disable: {disable})");
        ResetButtons(disable);
    }

    private void ResetButtons(bool disable = false)
    {
        foreach (var piece in puzzlePieces)
        {
            if (piece is BaseButton button)
            {
                if (disable)
                {
                    print($"disabling {piece.name}");
                    button.DisableButton();
                }
                else
                {
                    print($"force deactivating {piece.name}");
                    button.ForceDeactivate();
                }
            }
        }
    }

    // children implement when the puzzle is complete
    protected abstract bool IsPuzzleConditionMet();

    protected virtual void OnDestroy()
    {
        foreach (var piece in puzzlePieces)
        {
            if (piece != null)
            {
                piece.OnStateChanged -= OnPieceStateChanged;
            }
        }
    }
}
