using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBed : MonoBehaviour
{
    [SerializeField] float jumpingPower;

    public static event Action<float> OnJumpBoardHit;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ä§´ë");
            OnJumpBoardHit?.Invoke(jumpingPower);
        }
    }

}
