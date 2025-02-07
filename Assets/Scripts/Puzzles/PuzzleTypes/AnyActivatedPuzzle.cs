using System.Linq;
using UnityEngine;

public class AnyActivatedPuzzle : BasePuzzle
{
    protected override bool IsPuzzleConditionMet()
    {
        // Puzzle is solved when any piece is activated
        return puzzlePieces.Any(piece => piece.IsActivated);
    }
}
