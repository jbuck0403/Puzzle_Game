using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class VolumeSlider : BaseSlider
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private string volumeParameter = MixerChannels.Master;

    private VolumeSettings volumeSettings;

    private Coroutine saveAfterDelay;

    private void Start()
    {
        if (volumeSettings == null)
        {
            volumeSettings = FindObjectOfType<VolumeSettings>();
        }

        float initialValue = GetSliderValue();
        slider.value = initialValue;
        previousValue = initialValue;

        volumeSettings.onVolumeChanged.AddListener(
            (string channel, float newVolume) => SetSliderValue(channel, newVolume)
        );
    }

    private void OnDestroy()
    {
        if (saveAfterDelay != null)
        {
            StopCoroutine(saveAfterDelay);
        }
        if (volumeSettings != null)
        {
            volumeSettings.SaveSettings();
        }
    }

    private float GetSliderValue()
    {
        if (volumeSettings == null)
            return 1f;

        switch (volumeParameter)
        {
            case MixerChannels.Master:
                return volumeSettings.masterVolume;
            case MixerChannels.SFX:
                return volumeSettings.sFXVolume;
            case MixerChannels.Music:
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
        if (volumeSettings == null)
        {
            Debug.LogError("Cannot change volume: VolumeSettings is null");
            return;
        }

        volumeSettings.SetVolume(volumeParameter, value);

        // debounce the save operation
        if (saveAfterDelay == null)
        {
            saveAfterDelay = StartCoroutine(SaveAfterDelay());
        }
        else
        {
            StopCoroutine(saveAfterDelay);

            saveAfterDelay = StartCoroutine(SaveAfterDelay());
        }
    }

    private IEnumerator SaveAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        Debug.Log($"Saving {volumeParameter} volume setting: {slider.value}");
        volumeSettings.SaveSettings();
        saveAfterDelay = null;
    }
}
