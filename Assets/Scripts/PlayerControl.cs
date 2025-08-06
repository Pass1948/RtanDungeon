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


    Rigidbody rb;
    Vector3 dir;
    GameObject scanOdj;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� �Ⱥ��̰�
        curJumpP = jumpPower;
        curSpeed = speed;
    }

    void FixedUpdate()
    {
        Move();
        RayHit();
    }

    //==============[Move]==============
    void Move()
    {
        Vector3 movement = new Vector3(dir.x, 0f, dir.z).normalized;
        if (movement.sqrMagnitude > 1f) movement.Normalize();
        Vector3 delta = movement * curSpeed * Time.fixedDeltaTime;
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
        rb.AddForce(Vector2.up * curJumpP, ForceMode.Impulse);
    }


    //==============[Ray]==============
    void RayHit()
    {
        Debug.DrawRay(transform.position, transform.forward * 5f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, transform.forward, 5f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanOdj = rayHit.collider.gameObject;
        }
        else
            scanOdj = null;
    }
}

