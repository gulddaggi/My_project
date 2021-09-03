using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloser : MonoBehaviour
{
    [SerializeField]
    private ItemPickUp itemPickUp;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Animator door;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            this.door.SetTrigger("Close");
            itemPickUp.isCard = false;

        }

    }
}
