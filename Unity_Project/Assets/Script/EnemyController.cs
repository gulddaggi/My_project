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
    private bool isDead;
    private bool isShoot;
    public bool isStickAttack = false;

    //애니메이션
    public Animator anim;

    [SerializeField]
    private Rigidbody rigid;

    [SerializeField]
    private BoxCollider boxcol;

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

    public bool isPlayerInRange;


    void Start()
    {
        Stick.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        MoveCheck();
    }

    //움직임 확인
    private void MoveCheck()
    {
        if (!isRun)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalk = true;
            }
            else if (Vector3.Distance(lastPos, transform.position) < 0.01f)
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

    private void GunSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    IEnumerator ShotCoroutine()
    {
        isPlayerInRange = true;
        Waypoint.AttackNAvSetting();
        anim.SetTrigger("Attack");
        GunSound(gunSound);


        yield return new WaitForSeconds(shotDelayA);

        playerCon.DecreaseHp(gunDamage);
        Debug.Log(playerCon.hp);
            
        yield return new WaitForSeconds(shotDelayB);

        Waypoint.NavSettingReturn();

        yield return new WaitForSeconds(shotDelayA);

        isPlayerInRange = false;
    }

}
