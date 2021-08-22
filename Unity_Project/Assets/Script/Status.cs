using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    //ü��
    [SerializeField]
    private int hp;

    private int currentHp;

    //���׹̳�
    [SerializeField]
    private int sp;

    private int currentSp;

    [SerializeField]
    private int spRecharge;

    [SerializeField]
    private int spRechargeTime;

    private int currentRechargeTime;

    private bool isSpUsed;

    //����
    [SerializeField]
    private int shield;

    private int currentShield;

    //��Ʈ����
    [SerializeField]
    private int ent;

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
    }

    private void ImageUpdate()
    {
        images[0].fillAmount = (float)currentHp / hp;
        images[1].fillAmount = (float)currentSp / sp;
        images[2].fillAmount = (float)currentShield / shield;
        images[3].fillAmount = (float)currentEnt / ent;
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
            //���ӿ���
        }
    }

    public void IncreaseSp(int _count)
    {

    }

    public void DecreaseSp(int _count)
    {

    }

    public void IncreaseShield(int _Shield)
    {

    }

    public void DecreaseShield(int _damage)
    {

    }
}
