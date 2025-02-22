using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerTarget : BaseTarget
{
    [SerializeField]
    float timeToDisable = 5f;
    private Coroutine disableAfterTimeCoroutine;

    public override bool StartInteract(Transform interactor)
    {
        CleanUpCoroutine(disableAfterTimeCoroutine);

        if (!IsActivated)
        {
            base.StartInteract(interactor);
        }

        if (IsActivated)
        {
            disableAfterTimeCoroutine = StartCoroutine(DisableAfterTime(timeToDisable));
        }

        DestroyProjectile(interactor);
        return IsActivated;
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

    public override void ForceDeactivate()
    {
        CleanUpCoroutine(disableAfterTimeCoroutine);
        base.ForceDeactivate();
    }
}
