using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public int CollectedKeys { private set; get; }

    public bool CollectKey()
    {
        CollectedKeys++;
        return true;
    }

    public bool UseKey()
    {
        if (CollectedKeys > 0)
        {
            CollectedKeys--;
            return true;
        }

        return false;
    }
}
