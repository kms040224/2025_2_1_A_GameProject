using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("이동 설정")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10;

    [Header("점프 설정")]
    public float jumpHeight = 2f;                   //점프 높이
    public float gravity = -9.81f;                  //중력 추가
    public float landingDuration = 0.3f;            //착지 후 착지 지속시간


    [Header("공격 설정")]
    public float attackDuration = 0.8f;                 //공격 지속시간
    public bool canMeveWhlieAttacking = false;          //공격중 이동 가능 여부


    [Header("컴포넌트")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;


    //현재 상태
    private float currentSpeed;
    private bool isAttacking = false;               //공격중인지 체크
    private bool isLanding = false;                 //착지 중인지 확인
    private float landingTimer;                     //착지 시간 체크하는 착지 타이머

    private Vector3 velocity;
    private bool wasGrounded;                       //이전 프레임에 플레이어가 땅이었는지
    private bool isGrounded;                
    private float attackTimer;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        HandleLanding();
        HandleMovement();
        HandleJump();
        HandleAttack();
        UpdateAnimator();

    }

    void CheckGrounded()
    {
        //이전 상태 저장
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;                     //캐릭터 컨트롤러에서 받아온다.

        if(!isGrounded && wasGrounded)                          //땅에서 떨어졌을때 (지금 프레임은 땅이 아니고 이전 프레임은 땅인 경우)
        {
            Debug.Log("떨어지기 시작");
        }    

        if(isGrounded && velocity.y < 0)            //위에서 아래로 떨어지는 상황
        {
            velocity.y = -2f;

            //착지 모션 트리거 및 착지 상태 시작
            if(!wasGrounded && animator != null)
            {
                animator.SetTrigger("landTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("착지");
                    
            }
        }
    }

    void HandleLanding()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;             //랜덤 타이머 시간 만큼 못 움직임
            if(landingTimer <= 0)
            {
                isLanding = false;
            }
        }
    }

    void HandleAttack()
    {
        if(isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0)
            {
                isAttacking = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
        {
            isAttacking = true;                                         //공격중 표시
            attackTimer = attackDuration;

            if(animator != null)
            {
                animator.SetTrigger("attack");
            }
        }
    }

    void HandleJump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)           //땅 위에 있을때만 점프 가능
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if(animator != null)
            {
                animator.SetTrigger("jumpTrigger");
            }
        }

        if(!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()
    {
        if((isAttacking && !canMeveWhlieAttacking )|| isLanding)                    //공격중이나 착지중일때 움직임 제한
        {
            currentSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(horizontal !=0 || vertical != 0)                                     //둘 중 하나라도 입력이 있을때
        {
            //카메라가 보는 방향이 앞쪽으로 되게 설정
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;      //이동 방향 설정

            if(Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);         //캐릭터 컨트롤러의 이동 입력

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;                                   //이동이 아닐 경우 스피드 0
        }
    }

    void UpdateAnimator()
    {
        //전체 최대 속도(runSpeed)를 기준으로 0~1 계산
        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);   
        animator.SetFloat("speed", animatorSpeed);
        animator.SetBool("isGrounded", isGrounded);

        bool isFalling = !isGrounded && velocity.y < -0.1f;     //캐릭터의 y축 속도가 음수일때 떨어지고 있다고 판단
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);
    }
}
