using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private string gunName; //�̸�

    [SerializeField]
    private float range; //�����Ÿ�

    [SerializeField]
    private float accuracy; //��Ȯ��

    [SerializeField]
    private float fireRate; //����ӵ�

    [SerializeField]
    private float reloadTime; //������ �ӵ�

    [SerializeField]
    private int damage; //������

    [SerializeField]
    private int reloadBullet; //������ �Ѿ� ����

    [SerializeField]
    private int remainBullet; //���� ź������ �����ִ� �Ѿ� ����

    [SerializeField]
    private int maximumBullet; //�ִ� �Ѿ� ����

    [SerializeField]
    private int carryBullet; //���� �������� �Ѿ� ����(��������)

    [SerializeField]
    private float retroActionForce; //�ݵ�

    [SerializeField]
    private float retroActionFineSightForce; //�����ؽ� �ݵ�

    [SerializeField]
    private Vector3 AimOriginPos; //�����ؽ� ��ġ

    [SerializeField]
    private Vector3 originPos; //���� ��ġ

    [SerializeField]
    private Animator anim; //�ִϸ��̼�

    [SerializeField]
    private ParticleSystem muzzleFlash; //�ѱ�����

    [SerializeField]
    private AudioClip fire_Sound; //�߻���

    [SerializeField]
    private AudioClip dontReload_Sound; //������ �Ұ���

    [SerializeField]
    private AudioClip Reload_Sound; //��������

    private float fireRateValue; //����� �߻�ӵ� ��

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

    //�߻� �ӵ� ����
    private void FireRateCalc()
    {
        if (fireRateValue > 0)
        {
            fireRateValue -= Time.deltaTime;
        }
    }

    //�߻� ����
    private void BeforeFire()
    {
        if (Input.GetButton("Fire1") && fireRateValue <= 0 && !isReload)
        {
            Fire();
        }
    }

    //�߻� ���ɿ��� Ȯ��
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

    //�߻�
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

    //ȿ����
    private void GunSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }


    //������ �ڷ�ƾ
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
            Debug.Log("�Ѿ� ����");
        }
    }
}
