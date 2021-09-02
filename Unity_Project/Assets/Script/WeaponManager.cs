using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //무기 교체 여부
    public static bool isChangeWeapon = false;

    public static Transform currentWeaponTr; //현재 무기 정보
    public static Animator currentWeaponAnim; //현재 무기 애니메이션

    [SerializeField]
    private string currentWeapon; // 현재 무기 이름


    //교체 시작
    [SerializeField]
    private float changeWeaponOnDelay;

    //교체 끝
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

    [SerializeField]
    private ItemPickUp itemPickUp;




    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine(Weapon[0]));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && ItemPickUp.isHandgun)
            {
                StartCoroutine(ChangeWeaponCoroutine(Weapon[1]));
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && ItemPickUp.isStick)
            {
                StartCoroutine(ChangeWeaponCoroutine(Weapon[2]));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _name)
    {
        isChangeWeapon = true;

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
        else if (_name == "HandGun" && ItemPickUp.isHandgun)
        {
            handgun.HandGunChange();
            GunController.isGuncActivated = true;
        }
        else if (_name == "Stick" && ItemPickUp.isStick)
        {
            stick.StickChange();
            StickController.isStickActivated = true;
        }
    }
}
