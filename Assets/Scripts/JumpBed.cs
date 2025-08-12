using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBed : MonoBehaviour
{
    [SerializeField] float jumpingPower;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            GameManager.Character.JumpPlayer.controller.OnJumpBorad(jumpingPower);
        }
    }
}
