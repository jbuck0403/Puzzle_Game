using UnityEngine;

public class ReturnToMainMenuButton : TextButton
{
    [SerializeField]
    private PauseManager pauseManager;

    protected override void OnButtonPress()
    {
        if (pauseManager != null)
        {
            pauseManager.QuitToMainMenu();
        }
    }
}
