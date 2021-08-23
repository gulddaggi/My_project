using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    //�̸�
    [SerializeField]
    private string Stickname;

    //���ݹ���
    [SerializeField]
    private float range;

    //���ݷ�
    [SerializeField]
    private int damage;

    //���ݿ���
    private bool isAttack = false;

    //�ֵθ��⿩��
    private bool isSwing = false;

    //���� ������
    [SerializeField]
    private float totalAttackDelay;

    //���� Ȱ��ȭ ����
    [SerializeField]
    private float attackOnDelay;

    //���� ��Ȱ��ȭ ����
    [SerializeField]
    private float attackOutDelay;

    //�����ɽ�Ʈ
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;

    //�ִϸ��̼�
    public Animator anim;

    public static bool isStickActivated;

    private Crosshair crosshair;

    [SerializeField]
    private AudioClip stickSound;

    private AudioSource audioSource;

    [SerializeField]
    private EnemyController enemyCon;

    void Start()
    {
        crosshair = FindObjectOfType<Crosshair>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isStickActivated)
        {
            Attack();
        }
    }

    //���� ����
    private void Attack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
                crosshair.FireAnimation();
            }
        }
    }

    //���� ���� �ڷ�ƾ
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackOnDelay);
        isSwing = true;

        //�ǰ� ����
        StartCoroutine(HitCoroutine());
        StickSound(stickSound);

        yield return new WaitForSeconds(attackOutDelay);
        isSwing = false;

        yield return new WaitForSeconds(totalAttackDelay - attackOnDelay - attackOutDelay);
        isAttack = false;

    }

    //�ǰ� �ڷ�ƾ
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
                if (hitInfo.transform.tag == "Enemy")
                {
                    hitInfo.transform.GetComponent<EnemyController>().BeforeStickAttaked(damage);
                }
            }
            yield return null;
        }
    }

    //�ǰ� Ȯ��
    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Ÿ�ݽ� ȿ����
    private void StickSound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    //���ⱳü
    public void StickChange()
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
        isStickActivated = true;
    }

}
