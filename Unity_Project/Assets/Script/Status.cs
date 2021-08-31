using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    //체력
    [SerializeField]
    private int hp;

    private int currentHp;

    //스테미너
    [SerializeField]
    public int sp;

    private int currentSp;

    [SerializeField]
    private int spRecharge;

    [SerializeField]
    private int spRechargeTime;

    private int currentRechargeTime;

    private bool isSpUsed;

    //쉴드
    [SerializeField]
    private int shield;

    private int currentShield;

    [SerializeField]
    private int max_Shield;

    //엔트로피
    [SerializeField]
    private int ent;

    [SerializeField]
    private int max_Ent;

    private int currentEnt;

    [SerializeField]
    private Image[] images;



    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentShield = shield;
        currentEnt = ent;
    }

    void Update()
    {
        ImageUpdate();
        SPRechargeTime();
        SPRecharge();
    }

    private void ImageUpdate()
    {
        images[0].fillAmount = (float)currentHp / hp;
        images[1].fillAmount = (float)currentSp / sp;
        images[2].fillAmount = (float)currentShield / max_Shield;
        images[3].fillAmount = (float)currentEnt / max_Ent;
    }

    public void IncreaseHP(int _heal)
    {
        if (currentHp + _heal < hp)
        {
            currentHp += _heal;
        }
        else
        {
            currentHp = hp;
        }
    }

    public void DecreaseHP(int _damage)
    {
        if (currentShield > 0)
        {
            DecreaseShield(_damage);
            return;
        }
        currentHp -= _damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            //게임오버
        }
    }

    private void SPRechargeTime()
    {
        if (isSpUsed)
        {
            if (currentRechargeTime < spRechargeTime)
            {
                currentRechargeTime++;
            }
            else
            {
                isSpUsed = false;
            }
        }
    }

    private void SPRecharge()
    {
        if (!isSpUsed && currentSp < sp)
        {
            currentSp += spRecharge;
        }
    }

    public void DecreaseSp(int _count)
    {
        isSpUsed = true;
        currentRechargeTime = 0;

        if (currentSp - _count > 0)
        {
            currentSp -= _count;
        }
        else
        {
            currentSp = 0;
        }
    }

    public void IncreaseShield(int _Shield)
    {

        if (currentShield + _Shield < max_Shield)
        {
            currentShield += _Shield;
        }
        else
        {
            currentShield = max_Shield;
        }
    }

    public int GetSp()
    {
        return currentSp;
    }

    public void DecreaseShield(int _damage)
    {
        currentShield -= _damage;

        if (currentShield <= 0)
        {
            currentShield = 0;
        }
    }

    public void IncreaseEnt(int _ent)
    {
        if (currentEnt + _ent < max_Ent)
        {
            currentEnt += _ent;
        }
        else
        {
            currentEnt = max_Ent;
            
        }
    }

    public void EntOut()
    {
        currentEnt = 0;
    }
}
