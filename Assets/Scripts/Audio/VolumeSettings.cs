using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public float masterVolume;
    public float sFXVolume;
    public float musicVolume;
    private static string SettingsPath =>
        Path.Combine(Application.persistentDataPath, "settings.json");

    public UnityEvent<string, float> onVolumeChanged;

    private void Awake()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        if (File.Exists(SettingsPath))
        {
            string json = File.ReadAllText(SettingsPath);
            AudioSettings settings = JsonUtility.FromJson<AudioSettings>(json);
            ApplySettings(settings);
        }
    }

    private void SaveSettings()
    {
        AudioSettings settings = new AudioSettings
        {
            masterVolume = GetCurrentVolume("Master"),
            sfxVolume = GetCurrentVolume("SFX"),
            musicVolume = GetCurrentVolume("Music")
        };

        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(SettingsPath, json);
    }

    private float GetCurrentVolume(string parameter)
    {
        float dbValue;
        audioMixer.GetFloat(parameter, out dbValue);
        return Mathf.Pow(10, dbValue / 20f);
    }

    private void ApplySettings(AudioSettings settings)
    {
        masterVolume = settings.masterVolume;
        SetVolume("Master", masterVolume);

        sFXVolume = settings.sfxVolume;
        SetVolume("SFX", sFXVolume);

        musicVolume = settings.musicVolume;
        SetVolume("Music", musicVolume);
    }

    public void SetVolume(string channel, float volume)
    {
        float newVolume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(channel, newVolume);

        onVolumeChanged.Invoke(channel, volume);
        SaveSettings();
    }
}

[System.Serializable]
public class AudioSettings
{
    public float masterVolume = 1f;
    public float sfxVolume = 1f;
    public float musicVolume = 1f;
}
