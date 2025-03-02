using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField]
    private Light lightSource;

    [Header("Intensity Settings")]
    [SerializeField]
    private float baseIntensity = 1f;

    [SerializeField]
    private float intensityVariation = 0.5f;

    [Header("Timing Settings")]
    [SerializeField]
    private float minFlickerSpeed = 0.1f;

    [SerializeField]
    private float maxFlickerSpeed = 0.5f;

    [Header("Flicker Style")]
    [SerializeField]
    private bool smoothTransition = true;

    [SerializeField]
    [Range(0f, 1f)]
    private float flickerProbability = 0.5f;

    private float targetIntensity;
    private float currentIntensity;
    private float flickerTimer;
    private float currentFlickerSpeed;

    private void Start()
    {
        // if no light is assigned, try to get it from this GameObject
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();

            if (lightSource == null)
            {
                lightSource = GetComponentInChildren<Light>();
            }
        }

        if (lightSource == null)
        {
            Debug.LogError("No Light component found for FlickeringLight script!");
            enabled = false;
            return;
        }

        currentIntensity = baseIntensity;
        targetIntensity = baseIntensity;
        SetNewFlickerTarget();
    }

    private void Update()
    {
        flickerTimer -= Time.deltaTime;

        if (flickerTimer <= 0)
        {
            SetNewFlickerTarget();
        }

        if (smoothTransition)
        {
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * 10f);
        }
        else
        {
            currentIntensity = targetIntensity;
        }

        lightSource.intensity = currentIntensity;
    }

    private void SetNewFlickerTarget()
    {
        // only flicker sometimes based on probability
        if (Random.value > flickerProbability)
        {
            targetIntensity = baseIntensity;
        }
        else
        {
            // random intensity variation
            float randomIntensity = Random.Range(-intensityVariation, intensityVariation);
            targetIntensity = baseIntensity + randomIntensity;
        }

        // set random timer for next flicker
        currentFlickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);
        flickerTimer = currentFlickerSpeed;
    }
}
