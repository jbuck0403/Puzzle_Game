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
        // find the piece that just changed to activated
        var activatedPiece = puzzlePieces.FirstOrDefault(p =>
            p.IsActivated && !currentSequence.Contains(p)
        );

        if (activatedPiece != null)
        {
            // check if this is the next piece in the sequence
            int nextIndex = currentSequence.Count;
            if (nextIndex < requiredSequence.Count && activatedPiece == requiredSequence[nextIndex])
            {
                // correct next piece
                currentSequence.Add(activatedPiece);

                // If we completed the sequence, check puzzle solved and then reset
                if (currentSequence.Count == requiredSequence.Count)
                {
                    base.OnPieceStateChanged();
                    ResetPuzzle(true); // Disable all buttons after completion
                    return;
                }
            }
            else
            {
                // wrong piece - reset sequence
                ResetPuzzle();
                return;
            }
        }

        // Only check puzzle state if we haven't already handled it
        base.OnPieceStateChanged();
    }

    protected override bool IsPuzzleConditionMet()
    {
        // puzzle is solved when the current sequence matches the required sequence
        bool conditionMet = currentSequence.Count == requiredSequence.Count;

        if (conditionMet)
        {
            // Make sure to reset and disable all buttons when the puzzle is solved
            ResetPuzzle(true);
        }

        return conditionMet;
    }

    public override void ResetPuzzle(bool disable = false)
    {
        // Clear the current sequence first
        currentSequence.Clear();

        // Then reset all buttons
        foreach (var piece in puzzlePieces)
        {
            if (piece is BaseButton button)
            {
                if (disable)
                {
                    button.DisableButton();
                }
                else
                {
                    button.ForceDeactivate();
                }
            }
        }
    }
}
