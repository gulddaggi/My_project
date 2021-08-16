using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; //이름

    public float range; //사정거리

    public float accuracy; //정확도

    public float fireRate; //연사속도

    public int damage; //데미지

    public Animator anim; //애니메이션

    public ParticleSystem muzzleFlash; //총구섬광

}
