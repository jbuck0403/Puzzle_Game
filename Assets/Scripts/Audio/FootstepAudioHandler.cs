using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepAudioHandler : BaseAudioHandler
{
    [Header("Audio")]
    [SerializeField]
    private AudioClip[] defaultFootstepSounds;

    [SerializeField]
    private float minTimeBetweenSteps = 0.35f;

    [SerializeField]
    private float footstepVolume => soundVolume;

    [SerializeField]
    [Range(0.01f, 1.99f)]
    private float movementTolerance;

    private float timeSinceLastStep;
    private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
        audioSource.volume = footstepVolume;

        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("FootstepAudioHandler requires a PlayerInput component!");
        }

        if (defaultFootstepSounds == null || defaultFootstepSounds.Length == 0)
        {
            Debug.LogError("No footstep sounds assigned!");
        }
        else
        {
            Debug.Log($"Footsteps: {defaultFootstepSounds.Length} sound clips assigned");
        }
    }

    private void Update()
    {
        if (playerInput == null || audioSource == null)
            return;

        // only increment time when actually moving
        if (playerInput.inputSum > movementTolerance && !playerInput.jumping)
        {
            timeSinceLastStep += Time.deltaTime;
        }
        else
        {
            // reset the timer when not moving
            timeSinceLastStep = 0f;
        }

        // check if player is moving on the ground
        if (
            !playerInput.jumping
            && playerInput.inputSum > movementTolerance
            && timeSinceLastStep >= minTimeBetweenSteps
        )
        {
            PlayFootstep();
            timeSinceLastStep = 0f;
        }
    }

    private void PlayFootstep()
    {
        AudioClip soundToPlay = GetNextFootstepSound();
        if (soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay, footstepVolume);
        }
        else
        {
            Debug.LogError("Failed to get footstep sound to play");
        }
    }

    private AudioClip GetNextFootstepSound()
    {
        int footstepIndex = Random.Range(0, defaultFootstepSounds.Length);
        return defaultFootstepSounds[footstepIndex];
    }
}
