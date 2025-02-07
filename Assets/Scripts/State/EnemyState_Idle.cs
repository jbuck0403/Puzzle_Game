public class EnemyState_Idle : EnemyState
{
    public EnemyState_Idle(EnemyController enemy)
        : base(enemy) { }

    // initial behavior
    public override void OnStateEnter() { }

    // continuous behavior
    public override void OnStateUpdate() { }

    // switching to new state behavior
    public override void OnStateExit() { }
}
