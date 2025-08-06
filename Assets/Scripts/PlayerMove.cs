using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("플레이어 이동속도")]
    [SerializeField] float Speed;

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
    }

    private void OnMove(InputValue value)
    {
        dir.x = value.Get<Vector2>().x;
        dir.z = value.Get<Vector2>().y;
    }

    void Move()
    {
        rb.velocity = dir * curSpeed;
    }


}
