using UnityEngine;

public class RotationCurveManager : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve[] rotationCurves;

    [SerializeField]
    private float minSpeed = 0.5f;

    [SerializeField]
    private float maxSpeed = 2f;

    private float currentTime = 0f;
    private float currentSpeed;
    private int currentCurveIndex;
    private Vector3 currentRotationAxis;
    private AnimationCurve currentCurve;
    private bool isReversed = false;
    private float totalRotation = 0f;

    private void Start()
    {
        PickNewCurve();
    }

    private void Update()
    {
        if (rotationCurves == null || rotationCurves.Length == 0)
            return;

        // Progress the curve
        currentTime += Time.deltaTime * currentSpeed;

        // Get current rotation value from curve
        float curveValue = isReversed
            ? currentCurve.Evaluate(1f - currentTime)
            : currentCurve.Evaluate(currentTime);

        // Apply rotation
        transform.rotation = Quaternion.AngleAxis(curveValue * 360f, currentRotationAxis);

        // Check if curve is complete
        if (currentTime >= 1f)
        {
            PickNewCurve();
        }
    }

    private void PickNewCurve()
    {
        currentTime = 0f;

        // Pick random curve
        currentCurveIndex = Random.Range(0, rotationCurves.Length);
        currentCurve = rotationCurves[currentCurveIndex];

        // Pick random speed
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        // Generate random rotation axis
        currentRotationAxis = Random.onUnitSphere;

        // Toggle direction
        isReversed = !isReversed;
    }

    // Optional: Method to add curves at runtime
    public void AddCurve(AnimationCurve curve)
    {
        // Create new array with extra space
        AnimationCurve[] newCurves = new AnimationCurve[rotationCurves.Length + 1];
        rotationCurves.CopyTo(newCurves, 0);
        newCurves[rotationCurves.Length] = curve;
        rotationCurves = newCurves;
    }
}
