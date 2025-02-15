using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Puzzle Event", menuName = "Events/Puzzle Event")]
public class PuzzleEvent : ScriptableObject
{
    private UnityEvent _onEventRaised = new UnityEvent();

    public void RaiseEvent()
    {
        _onEventRaised.Invoke();
    }

    public void Subscribe(UnityAction listener)
    {
        _onEventRaised.AddListener(listener);
    }

    public void Unsubscribe(UnityAction listener)
    {
        _onEventRaised.RemoveListener(listener);
    }
}
