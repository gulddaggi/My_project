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

    // ���� ������ ����
    private Gun gun;

    private Crosshair crosshair;

    private RaycastHit hitInfo;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private ParticleSystem hit_effect;

    [SerializeField]
    private GameObject hit_effec;

    [SerializeField]
    private LayerMask layerMask;

    public static bool isGuncActivated; //Ȱ��ȭ����

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        transform.localPosition = originPos;
        gun = FindObjectOfType<Gun>();
        crosshair = FindObjectOfType<Crosshair>();
    }

    void Update()
    {
        if (isGuncActivated)
        {
            FireRateCalc();
            BeforeFire();
            BeforeReload();
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
        Hit();
        crosshair.FireAnimation();
        remainBullet--;
        GunSound(fire_Sound);
        fireRateValue = gun.fireRate;
        gun.muzzleFlash.Play();
        StopAllCoroutines();
    }

    public void Run(bool _bool)
    {
        gun.anim.SetBool("Run", _bool);
    }

    public void Walk(bool _bool)
    {
        gun.anim.SetBool("Walk", _bool);
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

    //����
    private void Aim()
    {
        isAim = !isAim;
        gun.anim.SetBool("Aim", isAim);
        crosshair.AimAnimation(isAim);

        
    }

    //���� ���
    public void CancleAim()
    {
        if (isAim)
        {
            Aim();
        }
    }

    //������ ���
    public void CancleReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    //�ǰ�
    private void Hit()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward + 
            new Vector3(Random.Range(-crosshair.Accuracy() - gun.accuracy, crosshair.Accuracy() + gun.accuracy), 
                        Random.Range(-crosshair.Accuracy() - gun.accuracy, crosshair.Accuracy() + gun.accuracy), 
                        0), 
                        out hitInfo, gun.range, layerMask))
        {
            GameObject clone = Instantiate(hit_effec, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            clone.GetComponent<ParticleSystem>().Play();
            Destroy(clone, 1.5f);
        }
    }

    //���� ��ü
    public void HandGunChange()
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
        isGuncActivated = true;

    }
}
