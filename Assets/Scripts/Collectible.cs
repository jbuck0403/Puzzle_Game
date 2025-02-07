using UnityEngine;

public abstract class Collectible : MonoBehaviour, IInteractable
{
    protected bool isCollected = false;

    public void SetCollected(bool value)
    {
        isCollected = value;
    }

    public bool CheckCollected()
    {
        return isCollected;
    }

    public virtual void StartInteract()
    {
        if (!isCollected)
        {
            OnCollect();
            isCollected = true;
        }
    }

    public virtual void EndInteract() { }

    protected abstract void OnCollect();
}
