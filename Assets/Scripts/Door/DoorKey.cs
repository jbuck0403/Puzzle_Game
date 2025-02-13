using UnityEngine;

public class DoorKey : Collectible
{
    protected override bool OnCollect(Transform interactor)
    {
        Debug.Log("DoorKey.OnCollect called");

        if (interactor == null)
        {
            Debug.LogWarning("Interactor is null!");
            return false;
        }

        KeyHandler keyHandler = interactor.GetComponent<KeyHandler>();
        if (keyHandler == null)
        {
            Debug.LogWarning($"No KeyHandler found on {interactor.name}!");
            return false;
        }

        Debug.Log("Found KeyHandler, attempting to collect");
        return keyHandler.CollectKey();
    }
}
