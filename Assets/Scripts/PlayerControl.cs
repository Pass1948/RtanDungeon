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


    Rigidbody rb;
    Vector3 dir;
    GameObject scanOdj;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 안보이게
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

