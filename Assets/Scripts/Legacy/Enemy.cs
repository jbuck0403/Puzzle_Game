using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform[] targetPoints;

    [SerializeField]
    private Transform enemy;

    [SerializeField]
    private float playerCheckDistance;

    [SerializeField]
    private float checkRadius = 0.4f;

    int currentTarget = 0;

    private NavMeshAgent agent;

    public bool isIdle;
    public bool isPlayerFound;
    public bool isCloseToPlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = targetPoints[currentTarget].position;
    }

    // void FixedUpdate()
    // {
    //     agent.Move(targetPoints[currentTarget].position);
    // }
}
