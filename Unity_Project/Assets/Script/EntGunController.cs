using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntGunController : MonoBehaviour
{
    public bool isAim = false;

    private bool isCharge = false;

    private float fireRateValue; //����� �߻�ӵ� ��

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
        if (Input.GetButton("Fire1") && fireRateValue == 0 && !isCharge)
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

    //�����߻�(���óġ)����
    private void BeforeChargeFire()
    {
        chargeGauge = 0;
        //�� ���� �Լ�

    }

    //ȿ����
    private void GunSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
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
        if (Physics.Raycast(cam.transform.position, cam.transform.forward +
            new Vector3(Random.Range(-crosshair.Accuracy() - gun.accuracy, crosshair.Accuracy() + gun.accuracy),
                        Random.Range(-crosshair.Accuracy() - gun.accuracy, crosshair.Accuracy() + gun.accuracy),
                        0),
                        out hitInfo, gun.range))
        {
            //�ǰݴ�� ����
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
