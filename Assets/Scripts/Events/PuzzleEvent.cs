using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Puzzle Event", menuName = "Events/Puzzle Event")]
public class PuzzleEvent : BaseEvent
{
    public void RaiseEvent()
    {
        _onEventRaised.Invoke();
    }

    public virtual void Subscribe(UnityAction listener)
    {
        _onEventRaised.AddListener(listener);
    }

    public virtual void Unsubscribe(UnityAction listener)
    {
        _onEventRaised.RemoveListener(listener);
    }
}
