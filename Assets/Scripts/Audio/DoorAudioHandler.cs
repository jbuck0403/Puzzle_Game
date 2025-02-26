using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorAudioHandler : BaseAudioHandler
{
    private AudioClip DoorOpenSound => activateSound;
    private AudioClip DoorCloseSound => deactivateSound;

    private float soundDuration;
    private BaseDoor door;
    private BaseMovable.MovementState lastMovementState;

    protected override void Awake()
    {
        base.Awake();

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
                PlaySound(DoorOpenSound);
            }
            else if (door.currentState == BaseMovable.MovementState.MovingToStart)
            {
                PlaySound(DoorCloseSound);
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
