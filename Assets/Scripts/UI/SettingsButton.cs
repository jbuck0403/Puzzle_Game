using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : TextButton
{
    [SerializeField]
    MenuManager menuManager;

    protected override void OnButtonPress()
    {
        menuManager.ShowSettings();
    }
}
