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
    private bool isDead = false;
    private bool isShoot;
    public bool isStickAttack = false;

    //�ִϸ��̼�
    public Animator anim;

    [SerializeField]
    private PlayerController playerCon;

    [SerializeField]
    private GameObject charge_Fire_Effect;

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

    [SerializeField]
    private EntGunController entGunCon;

    [SerializeField]
    private GunSensor gunSensor;

    [SerializeField]
    private AudioClip stickSound;

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
        }
        if (hp <= 0)
        {
            Dead();
        }

    }

    //������ Ȯ��
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
        audioSource.PlayOneShot(stickSound);
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
        if (!isPlayerInRange && !isStickAttack)
        {
            StartCoroutine(ShotCoroutine());
        }
    }

    //�����
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

    //��ƽ�ǰ�Ȯ��
    public void BeforeStickAttaked(int _damage)
    {
        DecreaseHp(_damage);
        StartCoroutine(StickAttackedCoroutine());

    }

    //��ƽ�ǰ��ڷ�ƾ
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

    //�����ǰ�
    public void HandGunAttacked(int _damage)
    {
        DecreaseHp(_damage);
    }

    //��Ʈ�������ǰ�
    public void EntGunAttacked()
    {
        if (!isEnt)
        {
            StartCoroutine(EntGunAttackedCoroutine());
        }
    }

    //��Ʈ�������ǰ��ڷ�ƾ
    IEnumerator EntGunAttackedCoroutine()
    {
        isEnt = true;
        yield return new WaitForSeconds(0.01f);

        Waypoint.AttackNAvSetting();
        anim.speed = 0f;

        yield return new WaitForSeconds(stopTime);

        isEnt = false;
        Waypoint.NavSettingReturn();
        anim.speed = 1f;
    }

    public void EntDead()
    {
        isEnt = false;
        Destroy(this.gameObject, 0.1f);
    }

    private void Dead()
    {
        Waypoint.AttackNAvSetting();
        GameObject clone = Instantiate(charge_Fire_Effect, transform.position, Quaternion.identity);
        Destroy(clone, 0.5f);
        Destroy(this.gameObject, 0.1f);
    }

    private int DecreaseHp(int _damage)
    {
        if (!isEnt)
        {
            hp -= _damage;
        }

        return hp;


    }

}
