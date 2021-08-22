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

    private void OnTriggerStay(Collider other)
    {
        if (other.transform == player && !enemyCon.isEnt)
        {
            isPlayerInRange = true;
            StickAttack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player && !enemyCon.isEnt)
        {
            isPlayerInRange = false;
            Waypoint.NavSettingReturn();
        }
    }

    private void StickAttack()
    {
        Vector3 direction = player.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        Physics.Raycast(ray, out hitInfo);

        if (hitInfo.collider.transform == player)
        {
            enemyCon.BeforeStickAttack();
        }
    }

}

