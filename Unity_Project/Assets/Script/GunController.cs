using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private float range; //사정거리

    [SerializeField]
    private float accuracy; //정확도

    [SerializeField]
    private float fireRate; //연사속도

    [SerializeField]
    private float reloadTime; //재장전 속도

    [SerializeField]
    private int damage; //데미지

    [SerializeField]
    private int reloadBullet; //재장전 총알 개수

    [SerializeField]
    private int remainBullet; //현재 탄알집에 남아있는 총알 개수

    [SerializeField]
    private int maximumBullet; //최대 총알 개수

    [SerializeField]
    private int carryBullet; //현재 소유중인 총알 개수

    [SerializeField]
    private float retroActionForce; //반동

    [SerializeField]
    private float retroActionFineSightForce; //정조준시 반동

    [SerializeField]
    private Vector3 fineSIghtOriginPos; //정조준시 위치

    [SerializeField]
    private Animator anim; //애니메이션

    [SerializeField]
    private ParticleSystem muzzleFlash; //총구섬광

    [SerializeField]
    private AudioClip fire_Sound; //발사음

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
