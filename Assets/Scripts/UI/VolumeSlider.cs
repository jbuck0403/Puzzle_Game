using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : BaseSlider
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private string volumeParameter = "Master"; // "Master", "SFX", or "Music"

    protected override void OnSliderValueChanged(float value)
    {
        // convert slider value (0 to 1) to decibels
        float dbValue = Mathf.Log10(value) * 20;
        audioMixer.SetFloat(volumeParameter, dbValue);
    }
}
