using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePuzzle : MonoBehaviour
{
    [SerializeField]
    protected PuzzleEvent OnPuzzleEvent;

    [SerializeField]
    public bool canBeUnsolved = false; // flag to allow puzzle to return to unsolved state

    [SerializeField]
    protected List<BasePuzzlePiece> externalPuzzlePieces = new List<BasePuzzlePiece>(); // pieces that belong to other puzzles

    [SerializeField]
    private bool disableOnReset = false;
    public event Action OnPuzzleStateChanged;
    protected List<BasePuzzlePiece> puzzlePieces = new List<BasePuzzlePiece>();

    protected bool isCompleted = false;
    public bool IsCompleted => isCompleted;

    public virtual void UnSolvePuzzle()
    {
        if (canBeUnsolved && isCompleted)
        {
            isCompleted = false;
            OnPuzzleEvent.RaiseUnsolvedEvent();
        }
    }

    protected virtual void Awake()
    {
        if (OnPuzzleEvent != null && !canBeUnsolved)
        {
            OnPuzzleEvent.Subscribe(() => ResetPuzzle(disableOnReset)); // only reset and disable buttons if puzzle cannot be unsolved
        }

        // Register any external puzzle pieces
        foreach (var piece in externalPuzzlePieces)
        {
            if (piece != null)
            {
                AddPuzzlePiece(piece);
            }
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
        bool conditionMet = IsPuzzleConditionMet();
        if (conditionMet && !isCompleted)
        {
            print("Puzzle Completed!");
            isCompleted = true;
            if (OnPuzzleEvent != null)
            {
                OnPuzzleEvent.RaiseSolvedEvent();
            }
        }
        else if (!conditionMet && isCompleted && canBeUnsolved)
        {
            UnSolvePuzzle();
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
