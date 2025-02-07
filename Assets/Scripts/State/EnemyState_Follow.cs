using UnityEngine;
using UnityEngine.AI;

public class EnemyState_Follow : EnemyState
{
    public Transform Target => enemyController.target;
    public NavMeshAgent Agent => enemyController.agent;

    public EnemyState_Follow(EnemyController enemy)
        : base(enemy) { }

    public override void OnStateEnter()
    {
        Agent.destination = GetTargetPosition();
    }

    public override void OnStateUpdate()
    {
        // if player in vision
        Agent.destination = GetTargetPosition();

        // if player still not in vision upon reaching last known location
        // return to previous patrol destination
    }

    public override void OnStateExit() { }

    private Vector3 GetTargetPosition()
    {
        return Target.position;
    }
}
