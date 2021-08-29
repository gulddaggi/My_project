using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEntGun : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed;

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0f, rotSpeed * Time.deltaTime, 0f));
    }
}
