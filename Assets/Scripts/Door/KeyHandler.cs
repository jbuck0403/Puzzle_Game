using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public int CollectedKeys { private set; get; }

    public bool CollectKey()
    {
        print("Key Collected");
        CollectedKeys++;
        return true;
    }

    public bool UseKey()
    {
        if (CollectedKeys > 0)
        {
            print("Key Used");
            CollectedKeys--;
            return true;
        }

        return false;
    }
}
