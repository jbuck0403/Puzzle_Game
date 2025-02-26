using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonAudioHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    private AudioClip activateSound;

    [SerializeField]
    private AudioClip deactivateSound;

    [SerializeField]
    private float soundVolume = 1f;

    [Header("Audio Settings")]
    [SerializeField]
    private float minDistance = 1f;

    [SerializeField]
    private float maxDistance = 5f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // Full 3D sound
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
        }
    }

    public void PlayActivateSound()
    {
        if (activateSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(activateSound, soundVolume);
        }
    }

    public void PlayDeactivateSound()
    {
        if (deactivateSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deactivateSound, soundVolume);
        }
    }
}
