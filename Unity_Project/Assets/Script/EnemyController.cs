using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private string enemyName;

    [SerializeField]
    private int hp;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;

    private float currentSpeed;

    public float stickDamage;


    [SerializeField]
    private GameObject rifle;

    [SerializeField]
    private GameObject Stick;

    private Vector3 direction;
    private Vector3 lastPos;

    private bool isWalk;
    private bool isRun;
    private bool isDead;
    private bool isShoot;
    public bool isStickAttack = false;

    public Animator anim;

    [SerializeField]
    private Rigidbody rigid;

    [SerializeField]
    private BoxCollider boxcol;

    [SerializeField]
    private PlayerController playerCon;

    [SerializeField]
    private float attackOnDelay;

    [SerializeField]
    private float attackOutDelay;

    private bool isSwing;

    [SerializeField]
    private float totalAttackDelay;
        
    void Start()
    {
        Stick.SetActive(false);
    }

    void Update()
    {
        MoveCheck();
    }

    //움직임확인
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

    //스틱으로 공격확인
    public void BeforeStickAttack()
    {
        if (!isStickAttack)
        {
            StopAllCoroutines();
            StartCoroutine(StickAttackCoroutine());
        }
    }

    IEnumerator StickAttackCoroutine()
    {
        isStickAttack = true;
        Stick.SetActive(true);
        rifle.SetActive(false);
        anim.SetTrigger("StickAttack");

        yield return new WaitForSeconds(attackOnDelay);
        isSwing = true;

        playerCon.StickAttacked();

        yield return new WaitForSeconds(attackOutDelay);
        isSwing = false;

        yield return new WaitForSeconds(totalAttackDelay - attackOnDelay - attackOutDelay);
        isStickAttack = false;
        Stick.SetActive(false);
        rifle.SetActive(true);
    }




}
