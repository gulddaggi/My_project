using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSensor : MonoBehaviour
{
    //시야각
    [SerializeField]
    private float viewAngle;

    //시야거리
    [SerializeField]
    private float viewRange;

    //플레이어 인식
    [SerializeField]
    private LayerMask targetMask;

    private EnemyController enemyCon;

    [SerializeField]
    private PointOfView POV;

    [SerializeField]
    private EnemyWaypoint waypoint;

    [SerializeField]
    private PlayerController playerCon;
    

    void Start()
    {
        enemyCon = GetComponent<EnemyController>();
    }

    void Update()
    {
        Sensor();
    }

    private Vector3 Boundary(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void Sensor()
    {
        Vector3 _leftBoundary = Boundary(-viewAngle * 0.5f);
        Vector3 _rightBoundary = Boundary(viewAngle * 0.5f);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewRange, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if (_targetTf.name == "Player")
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);
                if (_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hitInfo;

                    if (Physics.Raycast(transform.position, _direction, out _hitInfo, viewRange))
                    {
                        if (_hitInfo.transform.name == "Player" && !POV.isPlayerInRange)
                        {
                            enemyCon.BeforeSensorShot();
                        }
                    }
                }
            }
        }
    }

    

}
