using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool CanInteract { get; }
    float InteractRange { get; }

    public void StartInteract(Transform interactor) { }

    public void EndInteract() { }
}
