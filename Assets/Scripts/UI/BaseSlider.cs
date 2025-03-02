using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSlider : MonoBehaviour
{
    protected Slider slider;
    private float previousValue;

    protected virtual void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        if (slider == null)
        {
            slider = gameObject.AddComponent<Slider>();
        }

        // Store initial value
        previousValue = slider.value;

        // Add listener for value changes
        slider.onValueChanged.AddListener(HandleValueChanged);
    }

    private void HandleValueChanged(float value)
    {
        // Only trigger if value actually changed
        if (value != previousValue)
        {
            OnSliderValueChanged(value);
            previousValue = value;
        }
    }

    protected abstract void OnSliderValueChanged(float value);
}
