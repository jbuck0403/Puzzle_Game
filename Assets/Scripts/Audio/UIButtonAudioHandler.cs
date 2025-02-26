using UnityEngine;

public class UIButtonAudioHandler : BaseAudioHandler
{
    protected override void Awake()
    {
        base.Awake();
        if (audioSource != null)
        {
            audioSource.spatialBlend = 0f; // use 2D sound for UI
        }
    }

    public void PlayClickSound()
    {
        base.PlayActivateSound();
    }

    public void PlayHoverSound()
    {
        base.PlayDeactivateSound();
    }
}
