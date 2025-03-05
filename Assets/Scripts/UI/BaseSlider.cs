using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSlider : MonoBehaviour
{
    protected Slider slider;
    protected float previousValue;

    protected virtual void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        if (slider == null)
        {
            slider = gameObject.AddComponent<Slider>();
        }

        previousValue = slider.value;
        slider.onValueChanged.AddListener(HandleValueChanged);
    }

    private void HandleValueChanged(float value)
    {
        // only trigger if value actually changed
        if (value != previousValue)
        {
            OnSliderValueChanged(value);
            previousValue = value;
        }
    }

    protected abstract void OnSliderValueChanged(float value);
}
