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
    private EnemyController enemyCon;

    void Awake()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        if (!enemyCon.isShocked)
        {
            Patrol();
        }
    }

    public void Patrol()
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
