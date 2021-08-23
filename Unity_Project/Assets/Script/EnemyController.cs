using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //이름
    [SerializeField]
    private string enemyName;

    //체력
    [SerializeField]
    private int hp;

    //걷는속도
    [SerializeField]
    private float walkSpeed;

    //뛰는속도
    [SerializeField]
    private float runSpeed;

    //현재 속도
    private float currentSpeed;

    //근접공격 데미지
    public float stickDamage;

    //소총
    [SerializeField]
    private GameObject rifle;

    //전기충격봉
    [SerializeField]
    private GameObject Stick;

    //방향
    private Vector3 direction;

    private Vector3 lastPos;

    //상태확인
    private bool isWalk;
    private bool isRun;
    private bool isDead = false;
    private bool isShoot;
    public bool isStickAttack = false;

    //애니메이션
    public Animator anim;

    [SerializeField]
    private PlayerController playerCon;

    //공격시작 딜레이
    [SerializeField]
    private float attackOnDelay;

    //공격종료 딜레이
    [SerializeField]
    private float attackOutDelay;

    //공격 전체 딜레이
    [SerializeField]
    private float totalAttackDelay;

    //총 사정거리
    [SerializeField]
    private float gunRange;

    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;

    public AudioSource audioSource;

    [SerializeField]
    public AudioClip gunSound;

    [SerializeField]
    public float gunDamage;

    [SerializeField]
    public float shotDelayA;

    [SerializeField]
    public float shotDelayB;

    [SerializeField]
    private EnemyWaypoint Waypoint;

    [SerializeField]
    private EntGunController entGunCon;

    [SerializeField]
    private GunSensor gunSensor;

    [SerializeField]
    private float stopTime;

    public bool isPlayerInRange;

    public bool isShocked = false;

    public bool isEnt = false;


    [SerializeField]
    private Status status;

    void Start()
    {
        Stick.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isShocked && !isEnt)
        {
            MoveCheck();
        }
        else if (isEnt)
        {
            Waypoint.AttackNAvSetting();
            //StopAllCoroutines();
            //EntGunAttacked();
        }

    }

    //움직임 확인
    private void MoveCheck()
    {
        if (!isRun)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.001f)
            {
                isWalk = true;
            }
            else if (Vector3.Distance(lastPos, transform.position) < 0.001f)
            {
                isWalk = false;

            }
            lastPos = transform.position;
            anim.SetBool("Walk", isWalk);
        }
    }

    //근접공격 확인
    public void BeforeStickAttack()
    {
        if (!isStickAttack)
        {
            StartCoroutine(StickAttackCoroutine());
        }
    }

    //근접공격코루틴
    IEnumerator StickAttackCoroutine()
    {
        
        isStickAttack = true;
        Stick.SetActive(true);
        rifle.SetActive(false);
        anim.SetTrigger("StickAttack");
        Waypoint.AttackNAvSetting();

        yield return new WaitForSeconds(attackOnDelay);

        playerCon.StickAttacked();

        yield return new WaitForSeconds(attackOutDelay);

        Waypoint.NavSettingReturn();

        yield return new WaitForSeconds(totalAttackDelay - attackOnDelay - attackOutDelay);
        isStickAttack = false;
        Stick.SetActive(false);
        rifle.SetActive(true);
    }

    //적감지 시 
    public void BeforeSensorShot()
    {
        if (!isPlayerInRange)
        {
            StartCoroutine(ShotCoroutine());
        }
    }

    //사격음
    private void GunSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    IEnumerator ShotCoroutine()
    {
        isPlayerInRange = true;
        anim.SetTrigger("Attack");
        GunSound(gunSound);
        Waypoint.AttackNAvSetting();
        gunSensor.RotationSensor();


        yield return new WaitForSeconds(shotDelayA);

        status.DecreaseHP((int)gunDamage);
            
        yield return new WaitForSeconds(shotDelayB);

        Waypoint.NavSettingReturn();
        isPlayerInRange = false;
    }

    //스틱피격확인
    public void BeforeStickAttaked(int _damage)
    {
        DecreaseHp(_damage);
        StartCoroutine(StickAttackedCoroutine());

    }

    //스틱피격코루틴
    IEnumerator StickAttackedCoroutine()
    {
        isShocked = true;
        rifle.SetActive(false);
        anim.SetBool("Shock", isShocked);
        Waypoint.AttackNAvSetting();

        yield return new WaitForSeconds(3.0f);

        Waypoint.NavSettingReturn();
        isShocked = false;
        anim.SetBool("Shock", isShocked);
        rifle.SetActive(true);
    }

    //권총피격
    public void HandGunAttacked(int _damage)
    {
        DecreaseHp(_damage);
    }

    //엔트로피총피격
    public void EntGunAttacked()
    {
        if (entGunCon.chargeGauge < 100)
        {
            StartCoroutine(EntGunAttackedCoroutine());
        }
    }

    //엔트로피총피격코루틴
    IEnumerator EntGunAttackedCoroutine()
    {
        yield return new WaitForSeconds(0.01f);

        isEnt = true;
        Waypoint.AttackNAvSetting();
        anim.speed = 0f;

        yield return new WaitForSeconds(stopTime);

        isEnt = false;
        Waypoint.NavSettingReturn();
        anim.speed = 1f;
    }




    private int DecreaseHp(int _damage)
    {
        if (!isEnt)
        {
            hp -= _damage;
            Debug.Log(hp);
        }
        return hp;


    }

}
