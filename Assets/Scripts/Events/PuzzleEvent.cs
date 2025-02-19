using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Puzzle Event", menuName = "Events/Puzzle Event")]
public class PuzzleEvent : BaseEvent
{
    public void RaiseSolvedEvent() => RaisePrimaryEvent();

    public void RaiseUnSolvedEvent() => RaiseSecondaryEvent();
}
