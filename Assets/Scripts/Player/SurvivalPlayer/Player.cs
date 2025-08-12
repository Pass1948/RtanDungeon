using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControl controller;
    public PlayerCondition condition;
    public Equipment equipment;

    public ItemData ItemData;
    public Action addItem;

    public Transform dropPosition;

    private void Awake()
    {
        GameManager.Character.Player = this;
        controller = GetComponent<PlayerControl>();
        condition = GetComponent<PlayerCondition>();
        equipment = GetComponent<Equipment>();
    }
}
