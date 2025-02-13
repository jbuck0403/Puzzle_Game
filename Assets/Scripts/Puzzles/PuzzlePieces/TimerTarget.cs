using System.Collections;
using UnityEngine;

public class TimerTarget : BaseTarget
{
    [SerializeField]
    float timeToDisable = 5f;
    private Coroutine disableAfterTimeCoroutine;

    public override bool StartInteract(Transform interactor)
    {
        CleanUpCoroutine(disableAfterTimeCoroutine);

        // perform the actual pushing of the button
        bool result = base.StartInteract(interactor);

        if (IsActivated)
        {
            disableAfterTimeCoroutine = StartCoroutine(DisableAfterTime(timeToDisable));
        }

        return result;
    }

    private IEnumerator DisableAfterTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        DeactivateButton();
    }

    private void CleanUpCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    protected override void OnDisable()
    {
        CleanUpCoroutine(disableAfterTimeCoroutine);
        base.OnDisable();
    }
}
