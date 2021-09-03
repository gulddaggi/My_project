using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfView : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private EnemyWaypoint Waypoint;

    public bool isPlayerInRange = false;

    private RaycastHit hitInfo;

    [SerializeField]
    private EnemyController enemyCon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player && !enemyCon.isEnt && !isPlayerInRange)
        {
            StickAttack();
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player && !enemyCon.isEnt)
        {
            isPlayerInRange = false;

        }
    }

    private void StickAttack()
    {
            enemyCon.BeforeStickAttack();
        
    }

}

