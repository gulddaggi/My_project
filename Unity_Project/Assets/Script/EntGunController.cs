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
    private AudioClip charge_Fire_Sound;

    [SerializeField]
    private AudioClip charge_Alert;

    [SerializeField]
    private GameObject charge_Fire_Effect;

    [SerializeField]
    private GameObject uncharge_Fire_Effect;

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

    private PlayerController playerController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gun = FindObjectOfType<Gun>();
        crosshair = FindObjectOfType<Crosshair>();
        WeaponManager.currentWeaponTr = gun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = gun.anim;
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
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
        if (Input.GetButton("Fire1") && fireRateValue == 0 && !gameManager.isPause)
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
                                 
        Hit();

        crosshair.FireAnimation();
        
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
        if (hitInfo.transform.tag == "Enemy")
        {
            chargeGauge = 0;
            gun.anim.SetTrigger("Fire");
            audioSource.PlayOneShot(charge_Fire_Sound);
        }
        
    }

    //������ ����
    private void BeforeAim()
    {
        if (Input.GetButtonDown("Fire2") && !isCharge && !playerController.isRun)
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
                if (!isCharge)
                {
                    if (!hitInfo.transform.GetComponent<EnemyController>().isEnt)
                    {
                        hitInfo.transform.GetComponent<EnemyController>().EntGunAttacked();
                        ChargeCalc();
                        GameObject clone = Instantiate(uncharge_Fire_Effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                        Destroy(clone, 1.5f);
                    }
                }
                else if (isCharge)
                {
                    chargeGauge = 0;
                    gun.anim.SetTrigger("Fire");
                    audioSource.PlayOneShot(charge_Fire_Sound);
                    StartCoroutine(ReturnCoroutine());   

                }
                

            }

        }
    }

    IEnumerator ReturnCoroutine()
    {
        GameObject clone = Instantiate(charge_Fire_Effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        Destroy(clone, 1.5f);
        yield return null;
        if (hitInfo.transform.tag == "Enemy")
        {
            hitInfo.transform.GetComponent<EnemyController>().EntDead();
        }
        yield return null;
        isCharge = false;
        status.EntOut();
        chargeGauge = 0;
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
