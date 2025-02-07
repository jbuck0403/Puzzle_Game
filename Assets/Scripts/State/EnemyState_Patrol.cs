using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Patrol : EnemyState
{
    NavMeshAgent agent => enemyController.agent;
    Transform[] patrolPoints => enemyController.patrolPoints;
    int currentPatrolPoint => enemyController.currentPatrolPoint;

    public EnemyState_Patrol(EnemyController enemy)
        : base(enemy) { }

    // initial behavior
    public override void OnStateEnter()
    {
        agent.destination = patrolPoints[currentPatrolPoint].position;
    }

    // continuous behavior
    public override void OnStateUpdate()
    {
        if (!agent.pathPending && !agent.hasPath && agent.remainingDistance < 0.1f)
        {
            NextPatrolPoint();
            agent.destination = patrolPoints[currentPatrolPoint].position;
        }
    }

    // switching to new state behavior
    public override void OnStateExit() { }

    private void NextPatrolPoint()
    {
        if (currentPatrolPoint >= patrolPoints.Length - 1)
        {
            enemyController.currentPatrolPoint = 0;
        }
        else
        {
            enemyController.currentPatrolPoint++;
        }
    }
}
