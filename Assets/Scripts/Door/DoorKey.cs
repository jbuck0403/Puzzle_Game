using UnityEngine;

public class DoorKey : Collectible
{
    protected override bool OnCollect(Transform interactor)
    {
        KeyHandler keyHandler = interactor.GetComponent<KeyHandler>();
        if (keyHandler == null)
        {
            return false;
        }

        return keyHandler.CollectKey();
    }
}
