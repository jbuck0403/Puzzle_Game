public abstract class EnemyState
{
    protected EnemyController enemyController;

    public EnemyState(EnemyController enemy)
    {
        this.enemyController = enemy;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}

public enum States
{
    Idle,
    Patrol,
    Follow,
    Attack
}
