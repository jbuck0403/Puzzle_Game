using UnityEngine;

public abstract class Collectible : MonoBehaviour, IInteractable
{
    protected bool isCollected = false;
    public bool CanInteract { get; protected set; } = true;
    public virtual float InteractRange { get; } = InteractDistance.Short;

    public void SetCollected(bool value)
    {
        isCollected = value;
    }

    public bool CheckCollected()
    {
        return isCollected;
    }

    public virtual bool StartInteract(Transform interactor)
    {
        print("Interacting...");
        if (!isCollected)
        {
            print("Collecting...");
            if (OnCollect(interactor))
            {
                isCollected = true;
                CanInteract = false;
            }
        }

        return isCollected;
    }

    public virtual void EndInteract() { }

    protected abstract bool OnCollect(Transform interactor);
}
