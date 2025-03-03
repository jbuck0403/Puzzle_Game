using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class VolumeSlider : BaseSlider
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private string volumeParameter = "Volume";

    [SerializeField]
    private VolumeSettings volumeSettings;

    private void Start()
    {
        volumeSettings = FindAnyObjectByType<VolumeSettings>();
        float initialValue = GetSliderValue();
        slider.value = initialValue;
        previousValue = initialValue;

        volumeSettings.onVolumeChanged.AddListener(
            (string channel, float newVolume) => SetSliderValue(channel, newVolume)
        );
    }

    private float GetSliderValue()
    {
        switch (volumeParameter)
        {
            case "Master":
                return volumeSettings.masterVolume;
            case "SFX":
                return volumeSettings.sFXVolume;
            case "Music":
                return volumeSettings.musicVolume;
            default:
                return 1;
        }
    }

    private void SetSliderValue(string channel, float value)
    {
        if (channel == volumeParameter)
        {
            slider.value = value;
            previousValue = value;
        }
    }

    protected override void OnSliderValueChanged(float value)
    {
        volumeSettings.SetVolume(volumeParameter, value);
    }
}
