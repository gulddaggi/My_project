using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private AudioClip fire_Sound; //�߻���

    [SerializeField]
    private float reloadTime; //������ �ӵ�

    [SerializeField]
    private int reloadBullet; //������ �Ѿ� ����

    [SerializeField]
    private int remainBullet; //���� ź������ �����ִ� �Ѿ� ����

    [SerializeField]
    private int maximumBullet; //�ִ� �Ѿ� ����

    [SerializeField]
    private int carryBullet; //���� �������� �Ѿ� ����(��������)

    [SerializeField]
    private AudioClip dontReload_Sound; //������ �Ұ���

    [SerializeField]
    private AudioClip Reload_Sound; //��������

    [SerializeField]
    private Vector3 AimOriginPos; //�����ؽ� ��ġ

    [SerializeField]
    private Vector3 originPos; //���� ��ġ

    private float fireRateValue; //����� �߻�ӵ� ��

    private AudioSource audioSource;

    private bool isReload = false;

    public bool isAim = false;

    private Gun gun;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        transform.localPosition = originPos;
        gun = GetComponent<Gun>();
    }

    void Update()
    {
        FireRateCalc();
        BeforeFire();
        BeforeReload();
        BeforeAim();
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
        if (Input.GetButton("Fire1") && fireRateValue == 0 && !isReload)
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
                CancleAim();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    //�߻�
    private void Fire_On()
    {
        if (isAim)
        {
            gun.anim.SetTrigger("Aim_Fire");
        }
        else
        {
            gun.anim.SetTrigger("Fire");

        }
        remainBullet--;
        GunSound(fire_Sound);
        fireRateValue = gun.fireRate;
        gun.muzzleFlash.Play();
        StopAllCoroutines();
    }

    //ȿ����
    private void GunSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    //������ ����
    private void BeforeReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && remainBullet < reloadBullet)
        {
            CancleAim();
            StopAllCoroutines();
            StartCoroutine(ReloadCoroutine());
        }
    }


    //������ �ڷ�ƾ
    IEnumerator ReloadCoroutine()
    {
        if (carryBullet > 0)
        {
            isReload = true;

            carryBullet += remainBullet;

            gun.anim.SetTrigger("Reload");
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

    //������ ����
    private void BeforeAim()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            Aim();
        }
    }

    private void Aim()
    {
        isAim = !isAim;
        gun.anim.SetBool("Aim", isAim);

        if (isAim)
        {
            StartCoroutine(AimOnCoroutine());
        }
        else
        {
            StartCoroutine(AimOffCoroutine());
        }
    }

    public void CancleAim()
    {
        if (isAim)
        {
            Aim();
        }
    }

    IEnumerator AimOnCoroutine()
    {
        while (transform.localPosition != AimOriginPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, AimOriginPos, 0.2f);
            yield return null;
        }

    }
    IEnumerator AimOffCoroutine()
    {
        while (transform.localPosition != originPos)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }



}
