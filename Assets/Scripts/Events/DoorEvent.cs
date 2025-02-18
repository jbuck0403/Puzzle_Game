using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Door Event", menuName = "Events/Door Event")]
public class DoorEvent : BaseEvent
{
    private UnityEvent OnOpenEvent => _onEventRaised;
    private UnityEvent OnCloseEvent = new UnityEvent();

    public void RaiseOpenEvent()
    {
        OnOpenEvent.Invoke();
    }

    public void RaiseCloseEvent()
    {
        OnCloseEvent.Invoke();
    }

    // protected override void Subscribe(UnityAction openListener, UnityAction closeListener)
    // {
    //     SubscribeToOpen(openListener);
    //     SubscribeToClose(closeListener);
    // }

    // protected override void Unsubscribe(UnityAction openListener, UnityAction closeListener)
    // {
    //     UnsubscribeFromOpen(openListener);
    //     UnsubscribeFromClose(closeListener);
    // }

    public void SubscribeToOpen(UnityAction listener)
    {
        OnOpenEvent.AddListener(listener);
    }

    public void SubscribeToClose(UnityAction listener)
    {
        OnCloseEvent.AddListener(listener);
    }

    public void UnsubscribeFromOpen(UnityAction listener)
    {
        OnOpenEvent.RemoveListener(listener);
    }

    public void UnsubscribeFromClose(UnityAction listener)
    {
        OnCloseEvent.RemoveListener(listener);
    }
}
