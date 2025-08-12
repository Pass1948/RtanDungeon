using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlayerCotroller : MonoBehaviour
{
    [Header("플레이어 이동속도")]
    [SerializeField] float speed;
    float curSpeed;

    // 지면 체크용 레이 길이와 시작 위치 오프셋
    [SerializeField] float groundCheckDistance = 0.15f;
    [SerializeField] float groundCheckOffset = 0.1f;
    [SerializeField] LayerMask groundLayer; // "Ground" 레이어를 선택

    [Header("플레이어 점프파워")]
    [SerializeField] float jumpPower;
    [SerializeField] float curJumpP;


    [Header("화면 움직임")]
    [SerializeField] Transform cameraContainer;
    [SerializeField] float minXLook;
    [SerializeField] float maxXLook;
    [SerializeField] float lookSensitivity;

    [Header("카메라 전환")]
    [SerializeField] Camera thirdPersonCam;  // 3인칭 카메라
    [SerializeField] Camera firstPersonCam;  // 1인칭 카메라
    bool isFPS; // 현재 1인칭 여부


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
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 안보이게
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

        // 카메라 기준 방향 추출
        Vector3 camForward = cameraContainer.forward;
        Vector3 camRight = cameraContainer.right;

        // 수평 방향만 사용 (y축제거)
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
        // 마우스 움직임 기반 회전 처리
        float mouseX = mouseDir.x * lookSensitivity;
        float mouseY = mouseDir.y * lookSensitivity;

        // 수직 회전 - 카메라만 회전
        camCurXRot -= mouseY;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localRotation = Quaternion.Euler(camCurXRot, 0f, 0f);

        // 수평 회전 - 플레이어 본체 회전
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
        // 디버그용 레이
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);

        // 기존 4방향에서 1줄기로 수정
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);
    }

    public void OnJumpBorad(float jumpP)
    {
            rb.AddForce(Vector2.up * jumpP, ForceMode.Impulse);
    }

    //==============[시점변경버튼]==============
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
