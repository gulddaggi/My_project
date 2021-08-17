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

    [SerializeField]
    private Camera cam;

    private RaycastHit hitInfo;

    private AudioSource audioSource;

    private Gun gun;

    public static bool isEntGunActivated ;

    private Crosshair crosshair;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gun = FindObjectOfType<Gun>();
        crosshair = FindObjectOfType<Crosshair>();
        WeaponManager.currentWeaponTr = gun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = gun.anim;
    }

    void Update()
    {
        if (isEntGunActivated)
        {
            FireRateCalc();
            BeforeFire();
            BeforeAim();
        }
        
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
                BeforeChargeFire();
            }
        crosshair.FireAnimation();
        Hit();
        
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
        gun.muzzleFlash.Play();
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
        crosshair.AimAnimation(isAim);
    }

    public void CancleAim()
    {
        if (isAim)
        {
            Aim();
        }
    }

    private void Hit()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward +
            new Vector3(Random.Range(-crosshair.Accuracy() - gun.accuracy, crosshair.Accuracy() + gun.accuracy),
                        Random.Range(-crosshair.Accuracy() - gun.accuracy, crosshair.Accuracy() + gun.accuracy),
                        0),
                        out hitInfo, gun.range))
        {
            //피격대상 경직
        }
    }

    //무기교체
    public void EntGunChange()
    {
        //교체되는 무기 비활성화
        if (WeaponManager.currentWeaponTr != null)
        {
            WeaponManager.currentWeaponTr.gameObject.SetActive(false);
        }

        //교체할 무기의 정보 받기
        WeaponManager.currentWeaponTr = GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = GetComponent<Animator>();

        //교체할 무기 활성화
        gameObject.SetActive(true);
        isEntGunActivated = true;

    }
}
