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
            }
            else
            {
                // wrong piece - reset sequence
                ResetSequence();
                return;
            }
        }

        // check if puzzle is solved
        base.OnPieceStateChanged();
    }

    protected override bool IsPuzzleConditionMet()
    {
        // puzzle is solved when the current sequence matches the required sequence
        bool conditionMet = currentSequence.Count == requiredSequence.Count;

        if (!isCompleted && conditionMet)
        {
            ResetSequence(true);
        }

        return conditionMet;
    }

    private void ResetSequence(bool disable = false)
    {
        currentSequence.Clear();
        // deactivate all pieces
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
}
