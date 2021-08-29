using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntGunController : MonoBehaviour
{
    public bool isAim = false;

    private bool isCharge = false;

    public static bool isEntGunActivated = true;

    private float fireRateValue; //계산할 발사속도 값

    [SerializeField]
    private AudioClip fire_Sound;

    [SerializeField]
    private AudioClip charge_Alert;

    [SerializeField]
    public int chargeGauge = 0;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask layerMask;

    private RaycastHit hitInfo;

    private AudioSource audioSource;

    private Gun gun;


    private Crosshair crosshair;

    [SerializeField]
    private Status status;

    [SerializeField]
    private int entCount;

    [SerializeField]
    private int max_Ent;

    private GameManager gameManager;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gun = FindObjectOfType<Gun>();
        crosshair = FindObjectOfType<Crosshair>();
        WeaponManager.currentWeaponTr = gun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = gun.anim;
        gameManager = FindObjectOfType<GameManager>();
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
        if (Input.GetButton("Fire1") && fireRateValue == 0 && !isCharge && !gameManager.isPause)
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
        audioSource.PlayOneShot(fire_Sound);
        fireRateValue = gun.fireRate;
        gun.muzzleFlash.Play();

    }

    private void ChargeCalc()
    {
        chargeGauge += entCount;
        status.IncreaseEnt(entCount);
        if (chargeGauge >= max_Ent)
        {
            chargeGauge = max_Ent;
            isCharge = true;
            audioSource.PlayOneShot(charge_Alert);

        }
    }

    //충전발사(대상처치)상태
    private void BeforeChargeFire()
    {
        chargeGauge = 0;
        //적 멈춤 함수

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
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, gun.range, layerMask))
        {
            if (hitInfo.transform.tag == "Enemy")
            {
                hitInfo.transform.GetComponent<EnemyController>().EntGunAttacked();
                if (!hitInfo.transform.GetComponent<EnemyController>().isEnt)
                {
                    ChargeCalc();
                }

            }
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
