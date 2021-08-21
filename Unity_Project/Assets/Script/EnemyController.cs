using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //�̸�
    [SerializeField]
    private string enemyName;

    //ü��
    [SerializeField]
    private int hp;

    //�ȴ¼ӵ�
    [SerializeField]
    private float walkSpeed;

    //�ٴ¼ӵ�
    [SerializeField]
    private float runSpeed;

    //���� �ӵ�
    private float currentSpeed;

    //�������� ������
    public float stickDamage;

    //����
    [SerializeField]
    private GameObject rifle;

    //������ݺ�
    [SerializeField]
    private GameObject Stick;

    //����
    private Vector3 direction;

    private Vector3 lastPos;

    //����Ȯ��
    private bool isWalk;
    private bool isRun;
    private bool isDead;
    private bool isShoot;
    public bool isStickAttack = false;

    //�ִϸ��̼�
    public Animator anim;

    [SerializeField]
    private Rigidbody rigid;

    [SerializeField]
    private BoxCollider boxcol;

    [SerializeField]
    private PlayerController playerCon;

    //���ݽ��� ������
    [SerializeField]
    private float attackOnDelay;

    //�������� ������
    [SerializeField]
    private float attackOutDelay;

    //���� ��ü ������
    [SerializeField]
    private float totalAttackDelay;

    //�� �����Ÿ�
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

    //������ Ȯ��
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

    //�������� Ȯ��
    public void BeforeStickAttack()
    {
        if (!isStickAttack)
        {
            StartCoroutine(StickAttackCoroutine());
        }
    }

    //���������ڷ�ƾ
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

    //������ �� 
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
