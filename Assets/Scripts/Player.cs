using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControl controller;

    private void Awake()
    {
        GameManager.Character.Player = this;
        controller = GetComponent<PlayerControl>();
    }
}
