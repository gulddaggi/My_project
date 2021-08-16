using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private StickController Stick;
    private GunController Gun;
    private EntGunController EntGun;

    //이동
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    private float currentSpeed;

    private Rigidbody playerRigid;

    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;

    //움직임 체크
    private Vector3 lastPos;

    //점프
    [SerializeField]
    private float jumpForce;

    private CapsuleCollider capsuleCollider;

    //회전
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float rotationLimit;

    private float currentCameraRotationX = 0;

    [SerializeField]
    private Camera playerCamera;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerRigid = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
        Stick = FindObjectOfType<StickController>();
        Gun = FindObjectOfType<GunController>();
        EntGun = FindObjectOfType<EntGunController>();
    }


    void Update()
    {
        RotationY();
        RotationX();
        MoveCheck();

    }

    void FixedUpdate()
    {
        Jump();
        Move();
        Run();
        IsGround();
    }

    //이동
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
            }
            else if (Vector3.Distance(lastPos, transform.position) < 0.01f)
            {
                isWalk = false;
            }
            //Stick.anim.SetBool("Walk", isWalk);

            lastPos = transform.position;
        }
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f); //땅에 닿는다
    }

    //Y축 회전
    private void RotationY()
    {
        float _rotationX = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _rotationX * rotationSpeed;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -rotationLimit, rotationLimit);
        playerCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        
    }

    //X축 회전
    private void RotationX()
    {
        float _rotationY = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _rotationY, 0f) * rotationSpeed;
        playerRigid.MoveRotation(playerRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    //달리기
    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
            currentSpeed = runSpeed;
            //Gun.CancleAim();
            EntGun.CancleAim();

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            currentSpeed = walkSpeed;
        }
        //Stick.anim.SetBool("Run", isRun);
    }

    //점프
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            playerRigid.velocity = transform.up * jumpForce;
        }
    }

}
