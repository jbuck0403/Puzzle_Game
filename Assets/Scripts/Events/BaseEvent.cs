using UnityEngine;
using UnityEngine.Events;

public abstract class BaseEvent : ScriptableObject
{
    protected UnityEvent _onPrimaryEventRaised = new UnityEvent();
    protected UnityEvent _onSecondaryEventRaised = new UnityEvent();

    public void RaisePrimaryEvent()
    {
        _onPrimaryEventRaised.Invoke();
    }

    public void RaiseSecondaryEvent()
    {
        _onSecondaryEventRaised.Invoke();
    }

    public virtual void Subscribe(UnityAction primaryListener, UnityAction secondaryListener = null)
    {
        _onPrimaryEventRaised.AddListener(primaryListener);

        if (secondaryListener != null)
            _onSecondaryEventRaised.AddListener(secondaryListener);
    }

    public virtual void Unsubscribe(
        UnityAction primaryListener,
        UnityAction secondaryListener = null
    )
    {
        _onPrimaryEventRaised.RemoveListener(primaryListener);

        if (secondaryListener != null)
            _onSecondaryEventRaised.RemoveListener(secondaryListener);
    }
}
