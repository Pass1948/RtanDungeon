using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    private PlayerControl control;
    private PlayerCondition condition;


    private void Start()
    {
        control = GameManager.Character.Player.controller;
        condition = GameManager.Character.Player.condition;
    }

    public void EquipNew(ItemData data)
    {
        curEquip = Instantiate(data.equipPefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    void OnAttack()
    {
        curEquip.OnAttackInput();
    }
}
