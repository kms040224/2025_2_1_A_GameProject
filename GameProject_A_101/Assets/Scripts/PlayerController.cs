using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("�̵� ����")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10;

    [Header("���� ����")]
    public float attackDuration = 0.8f;                 //���� ���ӽð�
    public bool canMeveWhlieAttacking = false;          //������ �̵� ���� ����


    [Header("������Ʈ")]
    public Animator animator;

    private CharacterController controller;
    private Camera playerCamera;


    //���� ����
    private float currentSpeed;
    private bool isAttacking = false;               //���������� üũ


    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        UpdateAnimator();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(horizontal !=0 || vertical != 0)                                     //�� �� �ϳ��� �Է��� ������
        {
            //ī�޶� ���� ������ �������� �ǰ� ����
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;      //�̵� ���� ����

            if(Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);         //ĳ���� ��Ʈ�ѷ��� �̵� �Է�

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;                                   //�̵��� �ƴ� ��� ���ǵ� 0
        }
    }

    void UpdateAnimator()
    {
        //��ü �ִ� �ӵ�(runSpeed)�� �������� 0~1 ���
        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);   
        animator.SetFloat("speed", animatorSpeed);
    }
}
