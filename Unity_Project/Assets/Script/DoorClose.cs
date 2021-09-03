using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClose : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void DoorCloser()
    {
        anim.SetTrigger("Card");
    }
}
