using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntGunController : MonoBehaviour
{
    public bool isAim = false;

    private bool isCharge = false;

    private float fireRateValue; //계산할 발사속도 값

    [SerializeField]
    private AudioClip fire_Sound;

    [SerializeField]
    private AudioClip charge_Alert;

    [SerializeField]
    private int chargeGauge = 0;

    private AudioSource audioSource;

    private Gun gun;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gun = GetComponent<Gun>();
    }

    void Update()
    {
        FireRateCalc();
        BeforeFire();
        BeforeAim();
    }

    //발사 속도 제어
    private void FireRateCalc()
    {
        if (fireRateValue > 0)
        {
            fireRateValue -= Time.deltaTime;
            if (fireRateValue < 0)
            {
                fireRateValue = 0;
            }
        }
    }

    //발사 조건
    private void BeforeFire()
    {
        if (Input.GetButton("Fire1") && fireRateValue == 0 && !isCharge)
        {
            Fire();
        }
    }

    //발사 가능여부 확인
    private void Fire()
    {
            if (!isCharge)
            {
                UnChargeFire();
            }
            else
            {
                CancleAim();
                BeforeChargeFire();
            }
        
    }

    //미충전발사(대상정지)
    private void UnChargeFire()
    {
        if (isAim)
        {
            gun.anim.SetTrigger("Aim_Fire");
        }
        else
        {
            gun.anim.SetTrigger("Fire");

        }
        GunSound(fire_Sound);
        fireRateValue = gun.fireRate;
        //gun.muzzleFlash.Play();
        ChargeCalc();
        
    }

    private void ChargeCalc()
    {
        chargeGauge += 10;
        if (chargeGauge >= 100)
        {
            chargeGauge = 100;
            isCharge = true;
            GunSound(charge_Alert);
        }
    }

    //충전발사(대상처치)상태
    private void BeforeChargeFire()
    {
        chargeGauge = 0;
        //적 멈춤 함수

    }

    //효과음
    private void GunSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }


    //정조준 조건
    private void BeforeAim()
    {
        if (Input.GetButtonDown("Fire2") && !isCharge)
        {
            Aim();
        }
    }

    private void Aim()
    {
        isAim = !isAim;
        gun.anim.SetBool("Aim", isAim);
    }

    public void CancleAim()
    {
        if (isAim)
        {
            Aim();
        }
    }
}
