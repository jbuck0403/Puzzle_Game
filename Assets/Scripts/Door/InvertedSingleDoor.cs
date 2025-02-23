public class InvertedSingleDoor : SingleDoor
{
    public override void TriggerOpenDoor()
    {
        // Call the base class's close door method
        base.TriggerCloseDoor();
    }

    public override void TriggerCloseDoor()
    {
        // Call the base class's open door method
        base.TriggerOpenDoor();
    }

    public override void ForceOpenDoor()
    {
        UnlockDoor(true);
        TriggerOpenDoor(); // This will call our overridden method that actually closes the door
    }

    public override void ForceCloseDoor()
    {
        LockDoor(true);
        TriggerCloseDoor(); // This will call our overridden method that actually opens the door
    }
}
