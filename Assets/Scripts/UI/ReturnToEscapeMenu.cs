using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToEscapeMenu : TextButton
{
    [SerializeField]
    MenuManager menuManager;

    protected override void OnButtonPress()
    {
        menuManager.ShowEscapeMenu();
    }
}
