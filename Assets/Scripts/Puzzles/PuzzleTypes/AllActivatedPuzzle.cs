using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllActivatedPuzzle : BasePuzzle
{
    [SerializeField]
    private GameObject indicatorPrefab;

    [SerializeField]
    private Transform puzzleLightPosition;

    [SerializeField]
    private float spacing = 3f;

    [SerializeField]
    private Material pressedMaterial;

    [SerializeField]
    private Material defaultMaterial;

    private List<GameObject> indicators = new List<GameObject>();

    protected override bool IsPuzzleConditionMet()
    {
        // puzzle is solved when all pieces are activated
        return puzzlePieces.Count > 0 && puzzlePieces.All(piece => piece.IsActivated);
    }

    private Vector3 CalculateIndicatorPosition(int index)
    {
        float totalWidth = (puzzlePieces.Count - 1) * spacing;
        float startOffset = -totalWidth / 2f;
        return puzzleLightPosition.position
            + (puzzleLightPosition.right * (startOffset + (index * spacing)));
    }

    private GameObject CreateIndicator(Vector3 position)
    {
        return Instantiate(
            indicatorPrefab,
            position,
            puzzleLightPosition.rotation,
            puzzleLightPosition
        );
    }

    private void SetupIndicator(GameObject indicator, BasePuzzlePiece piece)
    {
        indicator.GetComponent<MeshRenderer>().material = defaultMaterial;
        piece.OnStateChanged += () => UpdateIndicator(indicator, piece.IsActivated);
    }

    private void UpdateIndicator(GameObject indicator, bool isActivated)
    {
        MeshRenderer renderer = indicator.GetComponent<MeshRenderer>();
        renderer.material = isActivated ? pressedMaterial : defaultMaterial;
    }

    public override void AddPuzzlePiece(BasePuzzlePiece piece)
    {
        base.AddPuzzlePiece(piece);

        // Create indicator at the correct position based on piece index
        int pieceIndex = puzzlePieces.Count - 1;
        Vector3 position = CalculateIndicatorPosition(pieceIndex);
        GameObject indicator = CreateIndicator(position);
        indicators.Add(indicator);
        SetupIndicator(indicator, piece);
    }
}
