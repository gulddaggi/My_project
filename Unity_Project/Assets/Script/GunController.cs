using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private AudioClip fire_Sound; //발사음

    [SerializeField]
    private float reloadTime; //재장전 속도

    [SerializeField]
    private int reloadBullet; //재장전 총알 개수

    [SerializeField]
    private int remainBullet; //현재 탄알집에 남아있는 총알 개수

    [SerializeField]
    private int maximumBullet; //최대 총알 개수

    [SerializeField]
    private int carryBullet; //현재 소유중인 총알 개수(장전가능)

    [SerializeField]
    private AudioClip dontReload_Sound; //재장전 불가음

    [SerializeField]
    private AudioClip Reload_Sound; //재장전음

    [SerializeField]
    private Vector3 AimOriginPos; //정조준시 위치

    [SerializeField]
    private Vector3 originPos; //기존 위치

    private float fireRateValue; //계산할 발사속도 값

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
        if (Input.GetButton("Fire1") && fireRateValue == 0 && !isReload)
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
                CancleAim();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    //발사
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

    //효과음
    private void GunSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    //재장전 조건
    private void BeforeReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && remainBullet < reloadBullet)
        {
            CancleAim();
            StopAllCoroutines();
            StartCoroutine(ReloadCoroutine());
        }
    }


    //재장전 코루틴
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
            Debug.Log("총알 없음");
        }
    }

    //정조준 조건
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
