using UnityEngine;

public class DoorKey : MonoBehaviour, IInteractable
{
    private bool collected = false;

    public void KeyCollected()
    {
        collected = true;
    }

    public void StartInteract(Transform interactor)
    {
        if (!collected)
        {
            KeyHandler keyHandler = interactor.GetComponent<KeyHandler>();
            if (keyHandler == null)
            {
                return;
            }
            keyHandler.CollectKey();

            KeyCollected();
        }
    }

    public void EndInteract() { }
}
