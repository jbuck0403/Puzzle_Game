using UnityEngine;

public class KeyHandler : MonoBehaviour
{
    public int CollectedKeys { private set; get; }

    public void CollectKey()
    {
        CollectedKeys++;
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
