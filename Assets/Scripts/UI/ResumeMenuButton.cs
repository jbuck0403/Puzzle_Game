using UnityEngine;

public class ResumeMenuButton : TextButton
{
    [SerializeField]
    private PauseManager pauseManager;

    protected override void OnButtonPress()
    {
        if (pauseManager != null)
            pauseManager.ResumeGame();
    }
}
