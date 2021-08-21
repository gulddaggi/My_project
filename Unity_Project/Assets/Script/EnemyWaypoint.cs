using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWaypoint : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Transform[] waypoints;

    private int currentWaypoint;

    [SerializeField]
    private Transform goal;
       
    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
        //navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Patrol();
        //navMeshAgent.destination = goal.position;

    }

    private void Patrol()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    public void AttackNAvSetting()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
    }

    public void NavSettingReturn()
    {
        navMeshAgent.isStopped = false;

    }



}
