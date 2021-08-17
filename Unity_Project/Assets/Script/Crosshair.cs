using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject crosshair; //크로스헤어

    private float accuracy; //정확도

    [SerializeField]
    private GunController Gunc;

    public void WalkAnimation(bool _bool)
    {
        anim.SetBool("Walk", _bool);
    }

    public void RunAnimation(bool _bool)
    {
        anim.SetBool("Run", _bool);
    }

    public void AimAnimation(bool _bool)
    {
        anim.SetBool("Aim", _bool);
    }

    public void FireAnimation()
    {
        if (anim.GetBool("Walk"))
        {
            anim.SetTrigger("Walk_Fire");
        }
        else
        {
            anim.SetTrigger("Idle_Fire");
        }
    }

    public float Accuracy()
    {
        if (anim.GetBool("Walk"))
        {
            accuracy = 0.035f;
        }
        else if (Gunc.isAim)
        {
            accuracy = 0.001f;
        }
        else
        {
            accuracy = 0.06f;
        }

        return accuracy;
    }
}
