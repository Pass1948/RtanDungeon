using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("플레이어 이동속도")]
    [SerializeField] float Speed;

    GameObject scanOdj;

    float curSpeed;
    Rigidbody rb;
    Vector3 dir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        curSpeed = Speed;
    }

    void FixedUpdate()
    {
        Move();
        RayHit();
    }

    private void OnMove(InputValue value)
    {
        dir.x = value.Get<Vector2>().x;
        dir.z = value.Get<Vector2>().y;
    }

    void Move()
    {
        Vector3 movement = new Vector3(dir.x, 0f, dir.z);
        if (movement.sqrMagnitude > 1f) movement.Normalize();

        Vector3 delta = movement * curSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + delta);
    }
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

