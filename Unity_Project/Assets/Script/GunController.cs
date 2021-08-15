using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private string gunName; //이름

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
    private int carryBullet; //현재 소유중인 총알 개수(장전가능)

    [SerializeField]
    private float retroActionForce; //반동

    [SerializeField]
    private float retroActionFineSightForce; //정조준시 반동

    [SerializeField]
    private Vector3 AimOriginPos; //정조준시 위치

    [SerializeField]
    private Vector3 originPos; //기존 위치

    [SerializeField]
    private Animator anim; //애니메이션

    [SerializeField]
    private ParticleSystem muzzleFlash; //총구섬광

    [SerializeField]
    private AudioClip fire_Sound; //발사음

    [SerializeField]
    private AudioClip dontReload_Sound; //재장전 불가음

    [SerializeField]
    private AudioClip Reload_Sound; //재장전음

    private float fireRateValue; //계산할 발사속도 값

    private AudioSource audioSource;

    private bool isReload = false;

    private bool isAim = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        FireRateCalc();
        BeforeFire();
        //BeforeReload();
    }

    //발사 속도 제어
    private void FireRateCalc()
    {
        if (fireRateValue > 0)
        {
            fireRateValue -= Time.deltaTime;
        }
    }

    //발사 조건
    private void BeforeFire()
    {
        if (Input.GetButton("Fire1") && fireRateValue <= 0 && !isReload)
        {
            Fire();
        }
    }

    //발사 가능여부 확인
    private void Fire()
    {

        if (!isReload)
        {
            if (remainBullet > 0)
            {
                Fire_On();
            }
            else
            {
                //CancleAim();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    //발사
    private void Fire_On()
    {
        remainBullet--;
        GunSound(fire_Sound);
        fireRateValue = fireRate;
        muzzleFlash.Play();
        anim.SetTrigger("Fire");
        StopAllCoroutines();
        //StartCoroutine(RetroActionCoroutine());
    }

    //효과음
    private void GunSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }


    //재장전 코루틴
    IEnumerator ReloadCoroutine()
    {
        if (carryBullet > 0)
        {
            isReload = true;

            carryBullet += reloadBullet;

            anim.SetTrigger("Reload");
            GunSound(Reload_Sound);
            yield return new WaitForSeconds(reloadTime);

            if (carryBullet >= reloadBullet)
            {
                remainBullet = reloadBullet;
                carryBullet -= reloadBullet;
            }
            else
            {
                remainBullet = carryBullet;
                carryBullet = 0;
            }

            isReload = false;

        }
        else
        {
            GunSound(dontReload_Sound);
            Debug.Log("총알 없음");
        }
    }
}
