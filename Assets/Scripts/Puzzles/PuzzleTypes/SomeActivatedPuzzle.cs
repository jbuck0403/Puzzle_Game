using System.Linq;
using UnityEngine;

public class SomeActivatedPuzzle : BasePuzzle
{
    [SerializeField]
    private int numActivatedToPass = 3;
    private int numActivated;

    protected override bool IsPuzzleConditionMet()
    {
        numActivated = puzzlePieces.Count(piece => piece.IsActivated);

        return numActivated >= numActivatedToPass;
    }
}
