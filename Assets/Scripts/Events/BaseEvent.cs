using UnityEngine;
using UnityEngine.Events;

public abstract class BaseEvent : ScriptableObject
{
    protected UnityEvent _onEventRaised = new UnityEvent();
}
