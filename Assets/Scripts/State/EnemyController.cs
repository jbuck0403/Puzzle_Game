using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    public Transform[] patrolPoints;
    public int currentPatrolPoint;
    private EnemyState prevState = null;
    private EnemyState enemyState;
    public NavMeshAgent agent;
    public Transform target;

    public States DEBUG_STATE;

    public void ChangeState(States newState)
    {
        if (prevState != null)
        {
            prevState.OnStateExit();
        }

        switch (newState)
        {
            case States.Idle:
                enemyState = new EnemyState_Idle(this);
                break;

            case States.Follow:
                enemyState = new EnemyState_Follow(this);
                break;

            case States.Patrol:
                enemyState = new EnemyState_Patrol(this);
                break;

            default:
                Debug.Log($"Unhandled State: {newState}");
                break;
        }

        enemyState.OnStateEnter();
        prevState = enemyState;
    }

    void Start()
    {
        ChangeState(DEBUG_STATE);
    }

    void Update()
    {
        enemyState.OnStateUpdate();
    }
}
