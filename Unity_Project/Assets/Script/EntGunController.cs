using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntGunController : MonoBehaviour
{
    public bool isAim = false;

    private bool isCharge = false;

    public static bool isEntGunActivated = true;

    private float fireRateValue; //����� �߻�ӵ� ��

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

    //�߻� �ӵ� ����
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

    //�߻� ����
    private void BeforeFire()
    {
        if (Input.GetButton("Fire1") && fireRateValue == 0 && !isCharge && !gameManager.isPause)
        {
            Fire();
        }
    }

    //�߻� ���ɿ��� Ȯ��
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

    //�������߻�(�������)
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

    //�����߻�(���óġ)����
    private void BeforeChargeFire()
    {
        chargeGauge = 0;
        //�� ���� �Լ�

    }

    //������ ����
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

    //���ⱳü
    public void EntGunChange()
    {
        //��ü�Ǵ� ���� ��Ȱ��ȭ
        if (WeaponManager.currentWeaponTr != null)
        {
            WeaponManager.currentWeaponTr.gameObject.SetActive(false);
        }

        //��ü�� ������ ���� �ޱ�
        WeaponManager.currentWeaponTr = GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = GetComponent<Animator>();

        //��ü�� ���� Ȱ��ȭ
        gameObject.SetActive(true);
        isEntGunActivated = true;

    }
}
