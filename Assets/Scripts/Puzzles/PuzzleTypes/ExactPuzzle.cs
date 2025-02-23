using System.Collections.Generic;
using UnityEngine;

public class ExactPuzzle : BasePuzzle
{
    [SerializeField]
    private List<BasePuzzlePiece> requiredActivatedPieces = new List<BasePuzzlePiece>();

    [SerializeField]
    private List<BasePuzzlePiece> requiredDeactivatedPieces = new List<BasePuzzlePiece>();

    protected override bool IsPuzzleConditionMet()
    {
        // Check all pieces that should be activated
        foreach (var piece in requiredActivatedPieces)
        {
            if (!piece.IsActivated)
            {
                return false;
            }
        }

        // Check all pieces that should be deactivated
        foreach (var piece in requiredDeactivatedPieces)
        {
            if (piece.IsActivated)
            {
                return false;
            }
        }

        // If we got here, all pieces are in their required states
        return true;
    }

    public override void AddPuzzlePiece(BasePuzzlePiece piece)
    {
        // Only add to puzzlePieces if it's in either required list
        if (requiredActivatedPieces.Contains(piece) || requiredDeactivatedPieces.Contains(piece))
        {
            base.AddPuzzlePiece(piece);
        }
    }

    public override void ResetPuzzle(bool disable = false)
    {
        // Don't reset or disable anything - the pieces are already in their correct states
        // when this is called (since the puzzle was just solved)
    }

    private void OnValidate()
    {
        // Ensure no piece is in both lists
        List<BasePuzzlePiece> duplicates = new List<BasePuzzlePiece>();
        foreach (var piece in requiredActivatedPieces)
        {
            if (requiredDeactivatedPieces.Contains(piece))
            {
                duplicates.Add(piece);
            }
        }

        // Remove any duplicates from the deactivated list
        foreach (var duplicate in duplicates)
        {
            Debug.LogWarning(
                $"Puzzle piece {duplicate.name} was in both activated and deactivated lists. Removing from deactivated list."
            );
            requiredDeactivatedPieces.Remove(duplicate);
        }
    }
}
