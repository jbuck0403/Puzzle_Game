using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorAudioHandler : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    private AudioClip doorOpenSound;

    [SerializeField]
    private AudioClip doorCloseSound;

    [SerializeField]
    private float soundVolume = 1f;

    private AudioSource audioSource;
    private float soundDuration;
    private BaseDoor door;
    private BaseMovable.MovementState lastMovementState;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // Full 3D sound
            audioSource.volume = soundVolume;
        }

        door = GetComponent<BaseDoor>();
        if (door == null)
        {
            Debug.LogError("DoorAudioHandler requires a BaseDoor component!");
        }
    }

    private void Update()
    {
        if (door == null)
            return;

        // Check for state changes
        if (door.currentState != lastMovementState)
        {
            // If we're starting to move in either direction
            if (door.currentState == BaseMovable.MovementState.MovingToEnd)
            {
                PlaySound(doorOpenSound);
            }
            else if (door.currentState == BaseMovable.MovementState.MovingToStart)
            {
                PlaySound(doorCloseSound);
            }

            lastMovementState = door.currentState;
        }

        // Update sound progress with door movement
        if (audioSource != null && audioSource.isPlaying)
        {
            bool isMoving =
                door.currentState == BaseMovable.MovementState.MovingToEnd
                || door.currentState == BaseMovable.MovementState.MovingToStart;

            if (isMoving)
            {
                // Sync sound position with door movement
                float targetTime = door.moveProgress * soundDuration;
                // On first frame after playing, always set time
                // After that, only adjust if drift is significant
                if (
                    door.currentState != lastMovementState
                    || Mathf.Abs(audioSource.time - targetTime) > 0.1f
                )
                {
                    audioSource.time = targetTime;
                }
            }
            else
            {
                // Stop sound if door isn't moving
                audioSource.Stop();
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            // Stop any existing sound
            audioSource.Stop();

            audioSource.clip = clip;
            audioSource.volume = soundVolume;
            soundDuration = clip.length;
            audioSource.Play();
        }
    }
}
