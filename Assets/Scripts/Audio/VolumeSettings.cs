using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    private AudioSettings audioSettings;

    public float masterVolume;
    public float sFXVolume;
    public float musicVolume;
    private static string SettingsPath =>
        Path.Combine(Application.persistentDataPath, "settings.json");

    public UnityEvent<string, float> onVolumeChanged;

    private void Awake()
    {
        // load settings from json file
        LoadSettings();
    }

    private void Start()
    {
        // apply loaded settings if audiomixer loaded after the file was read
        ApplySettings(audioSettings);
    }

    public void LoadSettings()
    {
        print("loading...");
        if (File.Exists(SettingsPath))
        {
            try
            {
                string json = File.ReadAllText(SettingsPath);
                audioSettings = JsonUtility.FromJson<AudioSettings>(json);
                ApplySettings(audioSettings);
                Debug.Log("Audio settings loaded successfully");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading audio settings: {e.Message}");

                ApplyDefaultSettings();
            }
        }
        else
        {
            Debug.Log("No saved audio settings found, using defaults");
            ApplyDefaultSettings();
        }
    }

    private void ApplyDefaultSettings()
    {
        SetVolume(MixerChannels.Master, 1f);

        SetVolume(MixerChannels.SFX, 1f);

        SetVolume(MixerChannels.Music, 1f);
    }

    public void SaveSettings()
    {
        try
        {
            AudioSettings settings = new AudioSettings
            {
                masterVolume = masterVolume,
                sfxVolume = sFXVolume,
                musicVolume = musicVolume
            };

            string json = JsonUtility.ToJson(settings);
            File.WriteAllText(SettingsPath, json);
            Debug.Log("Audio settings saved successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving audio settings: {e.Message}");
        }
    }

    private float GetCurrentVolume(string channel)
    {
        float dbValue;
        audioMixer.GetFloat(channel, out dbValue);
        return Mathf.Pow(10, dbValue / 20f);
    }

    private void ApplySettings(AudioSettings settings)
    {
        masterVolume = settings.masterVolume;
        SetVolume(MixerChannels.Master, masterVolume);

        sFXVolume = settings.sfxVolume;
        SetVolume(MixerChannels.SFX, sFXVolume);

        musicVolume = settings.musicVolume;
        SetVolume(MixerChannels.Music, musicVolume);
    }

    public void SetVolume(string channel, float volume)
    {
        float newVolume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(channel, newVolume);

        switch (channel)
        {
            case MixerChannels.Master:
                masterVolume = volume;
                break;
            case MixerChannels.SFX:
                sFXVolume = volume;
                break;
            case MixerChannels.Music:
                musicVolume = volume;
                break;
        }

        onVolumeChanged.Invoke(channel, volume);
    }
}

[System.Serializable]
public class AudioSettings
{
    public float masterVolume = 1f;
    public float sfxVolume = 1f;
    public float musicVolume = 1f;
}

public static class MixerChannels
{
    public const string Master = "Master";
    public const string SFX = "SFX";
    public const string Music = "Music";
}
