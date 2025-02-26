using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseAudioHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    protected AudioClip activateSound;

    [SerializeField]
    protected AudioClip deactivateSound;

    [SerializeField]
    protected float soundVolume = 1f;

    [Header("Audio Settings")]
    [SerializeField]
    protected float minDistance = 1f;

    [SerializeField]
    protected float maxDistance = 5f;

    protected AudioSource audioSource;

    [Header("Pitch Variation")]
    [SerializeField]
    [Range(0f, 0.5f)]
    private float pitchFlex = 0f;

    [SerializeField]
    private float defaultPitch = 1f;

    protected virtual void Awake()
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

    public virtual void PlayActivateSound()
    {
        if (activateSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(activateSound, soundVolume);
            PitchSound();
        }
    }

    public virtual void PlayDeactivateSound()
    {
        if (deactivateSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deactivateSound, soundVolume);
            PitchSound();
        }
    }

    protected virtual void PitchSound()
    {
        audioSource.pitch = Random.Range(defaultPitch - pitchFlex, defaultPitch + pitchFlex);
    }
}
