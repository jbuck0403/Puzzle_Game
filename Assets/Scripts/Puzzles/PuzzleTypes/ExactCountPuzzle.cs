using System.Linq;
using UnityEngine;

public class ExactCountPuzzle : BasePuzzle
{
    [SerializeField]
    private int requiredActivationCount = 2;

    [Tooltip("If true, will log the current activation count when checking puzzle condition")]
    [SerializeField]
    private bool debugActivationCount = false;

    protected override bool IsPuzzleConditionMet()
    {
        int currentlyActivated = puzzlePieces.Count(piece => piece.IsActivated);

        if (debugActivationCount)
        {
            Debug.Log(
                $"ExactCountPuzzle: {currentlyActivated} pieces activated, requiring exactly {requiredActivationCount}"
            );
        }

        return currentlyActivated == requiredActivationCount;
    }

    private void OnValidate()
    {
        // Ensure requiredActivationCount is positive
        if (requiredActivationCount < 0)
        {
            Debug.LogWarning("Required activation count cannot be negative. Setting to 0.");
            requiredActivationCount = 0;
        }
    }
}
