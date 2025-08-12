using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlayerCotroller : MonoBehaviour
{
    [Header("�÷��̾� �̵��ӵ�")]
    [SerializeField] float speed;
    float curSpeed;

    // ���� üũ�� ���� ���̿� ���� ��ġ ������
    [SerializeField] float groundCheckDistance = 0.15f;
    [SerializeField] float groundCheckOffset = 0.1f;
    [SerializeField] LayerMask groundLayer; // "Ground" ���̾ ����

    [Header("�÷��̾� �����Ŀ�")]
    [SerializeField] float jumpPower;
    [SerializeField] float curJumpP;


    [Header("ȭ�� ������")]
    [SerializeField] Transform cameraContainer;
    [SerializeField] float minXLook;
    [SerializeField] float maxXLook;
    [SerializeField] float lookSensitivity;

    [Header("ī�޶� ��ȯ")]
    [SerializeField] Camera thirdPersonCam;  // 3��Ī ī�޶�
    [SerializeField] Camera firstPersonCam;  // 1��Ī ī�޶�
    bool isFPS; // ���� 1��Ī ����


    Vector2 mouseDir;
    float camCurXRot;
    public bool canLook = true;

    Rigidbody rb;
    Vector3 dir;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        JumpBed.OnJumpBoardHit += OnJumpBorad;
    }

    private void OnDisable()
    {
        JumpBed.OnJumpBoardHit -= OnJumpBorad;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� �Ⱥ��̰�
        curJumpP = jumpPower;
        curSpeed = speed;
    }
    void FixedUpdate()
    {
        Move();
        if (canLook)
        {
            CameraLook();
        }
    }
    private void Update()
    {
        IsGround();
    }

    //==============[Move]==============
    void Move()
    {
        Vector3 inputDir = new Vector3(dir.x, 0f, dir.z).normalized;

        // ī�޶� ���� ���� ����
        Vector3 camForward = cameraContainer.forward;
        Vector3 camRight = cameraContainer.right;

        // ���� ���⸸ ��� (y������)
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        Vector3 delta = moveDir * curSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + delta);
    }

    public void OnMove(InputValue value)
    {
        dir.x = value.Get<Vector2>().x;
        dir.z = value.Get<Vector2>().y;
    }

    //==============[ViewMouse]==============

    public void OnLook(InputValue value)
    {
        mouseDir = value.Get<Vector2>();
    }

    void CameraLook()
    {
        // ���콺 ������ ��� ȸ�� ó��
        float mouseX = mouseDir.x * lookSensitivity;
        float mouseY = mouseDir.y * lookSensitivity;

        // ���� ȸ�� - ī�޶� ȸ��
        camCurXRot -= mouseY;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localRotation = Quaternion.Euler(camCurXRot, 0f, 0f);

        // ���� ȸ�� - �÷��̾� ��ü ȸ��
        transform.Rotate(Vector3.up * mouseX);
    }

    //==============[Jump]==============
    void OnJump()
    {
        if (IsGround())
            rb.AddForce(Vector2.up * curJumpP, ForceMode.Impulse);
    }

    bool IsGround()
    {
        Vector3 origin = transform.position + Vector3.down * groundCheckOffset;
        // ����׿� ����
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);

        // ���� 4���⿡�� 1�ٱ�� ����
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);
    }

    public void OnJumpBorad(float jumpP)
    {
            rb.AddForce(Vector2.up * jumpP, ForceMode.Impulse);
    }

    //==============[���������ư]==============
    void OnInventory()
    {
        ToggleView();
    }
    public void ToggleView()
    {
        isFPS = !isFPS;
        SetState(isFPS);
    }

    void SetState(bool isFPS)
    {
        if (firstPersonCam) firstPersonCam.enabled = isFPS;
        if (thirdPersonCam) thirdPersonCam.enabled = !isFPS;
    }


}
