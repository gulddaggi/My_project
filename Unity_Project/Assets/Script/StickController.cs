using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    //이름
    [SerializeField]
    private string Stickname;

    //공격범위
    [SerializeField]
    private float range;

    //공격력
    [SerializeField]
    private int damage;

    //공격여부
    private bool isAttack = false;

    //휘두르기여부
    private bool isSwing = false;

    //공격 딜레이
    [SerializeField]
    private float totalAttackDelay;

    //공격 활성화 시점
    [SerializeField]
    private float attackOnDelay;

    //공격 비활성화 시점
    [SerializeField]
    private float attackOutDelay;

    //레이케스트
    private RaycastHit hitInfo;

    //애니메이션
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

    //공격 시작
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

    //공격 시작 코루틴
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackOnDelay);
        isSwing = true;

        //피격 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(attackOutDelay);
        isSwing = false;

        yield return new WaitForSeconds(totalAttackDelay - attackOnDelay - attackOutDelay);
        isAttack = false;

    }

    //피격 코루틴
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

    //피격 확인
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

    //무기교체
    public void StickChange()
    {
        //교체되는 무기 비활성화
        if (WeaponManager.currentWeaponTr != null)
        {
            WeaponManager.currentWeaponTr.gameObject.SetActive(false);
        }

        //교체할 무기의 정보 받기
        WeaponManager.currentWeaponTr = GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = GetComponent<Animator>();

        //교체할 무기 활성화
        gameObject.SetActive(true);
        isStickActivated = true;
    }

}
