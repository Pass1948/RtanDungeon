using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("�÷��̾� �̵��ӵ�")]
    [SerializeField] float speed;
    float curSpeed;


    [Header("�÷��̾� �����Ŀ�")]
    [SerializeField] float jumpPower;
    float curJumpP;


    [Header("ȭ�� ������")]
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
       Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� �Ⱥ��̰�
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

        // ī�޶� ���� ���� ����
        Vector3 camForward = camara.forward;
        Vector3 camRight = camara.right;

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
        camara.localRotation = Quaternion.Euler(camCurXRot, 0f, 0f);

        // ���� ȸ�� - �÷��̾� ��ü ȸ��
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








