using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{
    [SerializeField]
    private ItemPickUp itemPickUp;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Animator door;


    private void OnTriggerEnter(Collider other)
    {
        if (itemPickUp.isCard)
        {
            door.SetTrigger("Card");
        }

    }

}
