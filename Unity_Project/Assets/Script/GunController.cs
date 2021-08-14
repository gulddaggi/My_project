using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private float range; //�����Ÿ�

    [SerializeField]
    private float accuracy; //��Ȯ��

    [SerializeField]
    private float fireRate; //����ӵ�

    [SerializeField]
    private float reloadTime; //������ �ӵ�

    [SerializeField]
    private int damage; //������

    [SerializeField]
    private int reloadBullet; //������ �Ѿ� ����

    [SerializeField]
    private int remainBullet; //���� ź������ �����ִ� �Ѿ� ����

    [SerializeField]
    private int maximumBullet; //�ִ� �Ѿ� ����

    [SerializeField]
    private int carryBullet; //���� �������� �Ѿ� ����

    [SerializeField]
    private float retroActionForce; //�ݵ�

    [SerializeField]
    private float retroActionFineSightForce; //�����ؽ� �ݵ�

    [SerializeField]
    private Vector3 fineSIghtOriginPos; //�����ؽ� ��ġ

    [SerializeField]
    private Animator anim; //�ִϸ��̼�

    [SerializeField]
    private ParticleSystem muzzleFlash; //�ѱ�����

    [SerializeField]
    private AudioClip fire_Sound; //�߻���

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
