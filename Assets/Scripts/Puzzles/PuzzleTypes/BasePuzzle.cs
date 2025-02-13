using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePuzzle : MonoBehaviour
{
    // subscribe to this event to trigger what happens when the puzzle is solved
    public event Action OnPuzzleCompleted;

    public event Action OnPuzzleStateChanged;

    protected List<BasePuzzlePiece> puzzlePieces = new List<BasePuzzlePiece>();

    protected bool isCompleted = false;

    public bool IsCompleted => isCompleted;

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
            OnPuzzleCompleted?.Invoke();
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
