using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SequencePuzzle : BasePuzzle
{
    [SerializeField]
    private List<BasePuzzlePiece> requiredSequence = new List<BasePuzzlePiece>();

    private List<BasePuzzlePiece> currentSequence = new List<BasePuzzlePiece>();

    protected override void OnPieceStateChanged()
    {
        // Find the piece that just changed to activated
        var activatedPiece = puzzlePieces.FirstOrDefault(p =>
            p.IsActivated && !currentSequence.Contains(p)
        );

        if (activatedPiece != null)
        {
            // Check if this is the next piece in the sequence
            int nextIndex = currentSequence.Count;
            if (nextIndex < requiredSequence.Count && activatedPiece == requiredSequence[nextIndex])
            {
                // Correct next piece
                currentSequence.Add(activatedPiece);
            }
            else
            {
                // Wrong piece - reset sequence
                ResetSequence();
                return;
            }
        }

        // Check if puzzle is solved
        base.OnPieceStateChanged();
    }

    protected override bool IsPuzzleConditionMet()
    {
        // Puzzle is solved when the current sequence matches the required sequence
        return currentSequence.Count == requiredSequence.Count;
    }

    private void ResetSequence()
    {
        currentSequence.Clear();
        // Deactivate all pieces
        foreach (var piece in puzzlePieces)
        {
            if (piece is BasePressurePlate pressurePlate)
            {
                pressurePlate.ForceDeactivate();
            }
        }
    }
}
