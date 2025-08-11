using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("플레이어 이동속도")]
    [SerializeField] float speed;
    float curSpeed;


    [Header("플레이어 점프파워")]
    [SerializeField] float jumpPower;
    float curJumpP;


    [Header("화면 움직임")]
    [SerializeField] Transform camara;
    [SerializeField] float minXLook;
    [SerializeField] float maxXLook;
    [SerializeField] float lookSensitivity;
    Vector2 mouseDir;
    float camCurXRot;
    public bool canLook = true;
    public Action inventory;

    Rigidbody rb;
    Vector3 dir;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
    }

    private void LateUpdate()
    {
        if(canLook)
        {
            CameraLook();
        }
    }

    //==============[Move]==============
    void Move()
    {
        Vector3 inputDir = new Vector3(dir.x, 0f, dir.z).normalized;

        // 카메라 기준 방향 추출
        Vector3 camForward = camara.forward;
        Vector3 camRight = camara.right;

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
        camara.localRotation = Quaternion.Euler(camCurXRot, 0f, 0f);

        // 수평 회전 - 플레이어 본체 회전
        transform.Rotate(Vector3.up * mouseX);
    }

    //==============[Jump]==============
    void OnJump()
    {
        if(IsGround())
        rb.AddForce(Vector2.up * curJumpP, ForceMode.Impulse);
    }

    bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, LayerMask.GetMask("Ground")))
            {
                return true;
            }
        }
        return false;
    }

    //==============[Inventory]==============
    void OnInventory()
    {
        inventory?.Invoke();
        ToggleCursor();
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}








