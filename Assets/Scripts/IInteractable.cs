using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool CanInteract { get; }
    float InteractRange { get; }

    string PromptText => "Press E";

    public bool StartInteract(Transform interactor);

    public void EndInteract();
}
