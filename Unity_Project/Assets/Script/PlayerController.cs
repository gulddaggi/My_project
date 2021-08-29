using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private StickController Stick;
    private GunController Gunc;
    private EntGunController EntGun;
    private Gun Gun;

    //�̵�
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    private float currentSpeed;

    private Rigidbody playerRigid;

    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;

    //������ üũ
    private Vector3 lastPos;

    //����
    [SerializeField]
    private float jumpForce;

    private CapsuleCollider capsuleCollider;

    //ȸ��
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float rotationLimit;

    private float currentCameraRotationX;

    [SerializeField]
    private Camera playerCamera;

    private Crosshair crosshair;

    [SerializeField]
    public float hp;

    [SerializeField]
    private EnemyController EnemyCon;

    [SerializeField]
    private Status status;

    public bool isShock = false;

    private GameManager gameManager;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerRigid = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
        Stick = FindObjectOfType<StickController>();
        Gunc = FindObjectOfType<GunController>();
        EntGun = FindObjectOfType<EntGunController>();
        crosshair = FindObjectOfType<Crosshair>();
        Gun = FindObjectOfType<Gun>();
        gameManager = FindObjectOfType<GameManager>();

    }


    void Update()
    {
        if (!isShock && gameManager.canPlayerMove)
        {
            RotationY();
            RotationX();
            MoveCheck();
        }

    }

    void FixedUpdate()
    {
        if (!isShock && gameManager.canPlayerMove)
        {
            Jump();
            Move();
            Run();
            IsGround();
        }
        
    }

    //�̵�
    private void Move()
    {
        float _moveX = Input.GetAxisRaw("Horizontal");
        float _moveZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveX;
        Vector3 _moveVertical = transform.forward * _moveZ;

        Vector3 _moveDir = (_moveHorizontal + _moveVertical).normalized * currentSpeed;

        playerRigid.MovePosition(transform.position + _moveDir * Time.deltaTime);
    }

    private void MoveCheck()
    {
        if (!isRun && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalk = true;

                if (EntGunController.isEntGunActivated)
                {
                    WeaponManager.currentWeaponAnim.SetBool("Walk", isWalk);
                }
                else if (GunController.isGuncActivated)
                {
                    WeaponManager.currentWeaponAnim.SetBool("Walk", isWalk);
                }
                else if (StickController.isStickActivated)
                {
                    WeaponManager.currentWeaponAnim.SetBool("Walk", isWalk);
                }
            }
            else if (Vector3.Distance(lastPos, transform.position) < 0.01f)
            {
                isWalk = false;

                if (EntGunController.isEntGunActivated)
                {
                    WeaponManager.currentWeaponAnim.SetBool("Walk", isWalk);
                }
                else if (GunController.isGuncActivated)
                {
                    WeaponManager.currentWeaponAnim.SetBool("Walk", isWalk);
                }
                else if (StickController.isStickActivated)
                {
                    WeaponManager.currentWeaponAnim.SetBool("Walk", isWalk);
                }
            }
            crosshair.WalkAnimation(isWalk);
            lastPos = transform.position;
            
        }
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f); //���� ��´�
    }

    //Y�� ȸ��
    private void RotationY()
    {

        float _rotationX = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _rotationX * rotationSpeed;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -rotationLimit, rotationLimit);
        playerCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        
    }

    //X�� ȸ��
    private void RotationX()
    {
        float _rotationY = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _rotationY, 0f) * rotationSpeed;
        playerRigid.MoveRotation(playerRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    //�޸���
    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && status.GetSp() > 0)
        {
            isRun = true;
            currentSpeed = runSpeed;
            status.DecreaseSp(1);

            if (EntGunController.isEntGunActivated)
            {
                WeaponManager.currentWeaponAnim.SetBool("Run", isRun);
            }
            else if (GunController.isGuncActivated)
            {
                WeaponManager.currentWeaponAnim.SetBool("Run", isRun);
            }
            else if (StickController.isStickActivated)
            {
                WeaponManager.currentWeaponAnim.SetBool("Run", isRun);
            }

        }
        else if (!Input.GetKey(KeyCode.LeftShift) || status.GetSp() <= 0)
        {
            isRun = false;

            currentSpeed = walkSpeed;

            if (EntGunController.isEntGunActivated)
            {
                WeaponManager.currentWeaponAnim.SetBool("Run", isRun);
            }
            else if (GunController.isGuncActivated)
            {
                WeaponManager.currentWeaponAnim.SetBool("Run", isRun);
            }
            else if (StickController.isStickActivated)
            {
                WeaponManager.currentWeaponAnim.SetBool("Run", isRun);
            }

        }
        isRun = false;

        crosshair.RunAnimation(isRun);
    }

    //����
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            playerRigid.velocity = transform.up * jumpForce;
        }
    }

    public void StickAttacked()
    {
        StartCoroutine(StickAttackedCoroutine());
    }

    //�����κ��� ��ƽ���� �ڷ�ƾ
    IEnumerator StickAttackedCoroutine()
    {
        if (!EnemyCon.isEnt)
        {
            status.DecreaseHP((int)EnemyCon.stickDamage);
            isShock = true;

            yield return new WaitForSeconds(3.0f);
            Debug.Log(hp);
            isShock = false;
        }
       
    }

    //ü�°���
    public float DecreaseHp(float _damage)
    {
        hp -= _damage;
        return hp;
    }

}
