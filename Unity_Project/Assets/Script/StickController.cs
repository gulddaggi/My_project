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

    //�ִϸ��̼�
    public Animator anim;

    public static bool isStickActivated;

    private Crosshair crosshair;

    void Start()
    {
        crosshair = FindObjectOfType<Crosshair>();
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
            }
            yield return null;
        }
    }

    //�ǰ� Ȯ��
    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range))
        {
            return true;
        }
        else
        {
            return false;
        }

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
