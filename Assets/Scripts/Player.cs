using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControl controller;

    private void Awake()
    {
        controller = GetComponent<PlayerControl>();
        GameManager.Character.Player = this;
    }
}
