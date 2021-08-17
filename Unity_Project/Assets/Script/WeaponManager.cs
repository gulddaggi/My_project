using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //���� ��ü ����
    public static bool isChangeWeapon = false;

    public static Transform currentWeaponTr; //���� ���� ����
    public static Animator currentWeaponAnim; //���� ���� �ִϸ��̼�

    [SerializeField]
    private string currentWeapon; // ���� ���� �̸�


    //��ü ����
    [SerializeField]
    private float changeWeaponOnDelay;

    //��ü ��
    [SerializeField]
    private float changeWeaponOffDelay;

    [SerializeField]
    private GunController handgun;

    [SerializeField]
    private StickController stick;

    [SerializeField]
    private EntGunController entGun;

    [SerializeField]
    private string[] Weapon;


    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine(Weapon[0]));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine(Weapon[1]));
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(ChangeWeaponCoroutine(Weapon[2]));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _name)
    {
        isChangeWeapon = true;
        //currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponOnDelay);

        CancleWeaponAction();
        WeaponChange(_name);

        yield return new WaitForSeconds(changeWeaponOffDelay);


        currentWeapon = _name;
        isChangeWeapon = false;

    }

    private void CancleWeaponAction()
    {
        switch (currentWeapon)
        {
            case "EntGun":
                entGun.CancleAim();
                EntGunController.isEntGunActivated = false;
                break;

            case "HandGun":
                handgun.CancleAim();
                handgun.CancleReload();
                GunController.isGuncActivated = false;
                break;

            case "Stick":
                StickController.isStickActivated = false;
                break;
        }
    }

    private void WeaponChange(string _name)
    {
        if (_name == "EntGun")
        {
            entGun.EntGunChange();
            EntGunController.isEntGunActivated = true;
        }
        else if (_name == "HandGun")
        {
            handgun.HandGunChange();
            GunController.isGuncActivated = true;
        }
        else if (_name == "Stick")
        {
            stick.StickChange();
            StickController.isStickActivated = true;
        }
    }
}
