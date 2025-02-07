using System.Linq;
using UnityEngine;

public class AllActivatedPuzzle : BasePuzzle
{
    protected override bool IsPuzzleConditionMet()
    {
        // Puzzle is solved when all pieces are activated
        return puzzlePieces.Count > 0 && puzzlePieces.All(piece => piece.IsActivated);
    }
}
