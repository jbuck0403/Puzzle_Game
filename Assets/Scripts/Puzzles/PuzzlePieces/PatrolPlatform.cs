using UnityEngine;

public class PatrolPlatform : BasePlatform
{
    [SerializeField]
    private float pauseDuration = 1f;
    private float pauseTimer = 0f;

    protected override void Start()
    {
        base.Start();

        TriggerMoveToEnd();
    }

    protected override void Update()
    {
        // if we're at an endpoint, handle pausing
        if (currentState == MovementState.AtEnd || currentState == MovementState.AtStart)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                pauseTimer = 0f;
                // if we're at the end, move back to start, otherwise move to end
                if (currentState == MovementState.AtEnd)
                {
                    TriggerMoveToStart();
                }
                else
                {
                    TriggerMoveToEnd();
                }
            }
        }
        else
        {
            // Normal movement update
            base.Update();
        }
    }
}
