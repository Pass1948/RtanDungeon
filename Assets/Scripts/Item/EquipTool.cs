using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;

    [Header("�ڿ� ȹ��")]
    public bool doesGatherRespurces;

    [Header("����")]
    public bool doseDealDamage;
    public int damage;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnAttackInput()
    {
        if(!attacking)
        {
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }

    }

    void OnCanAttack()
    {
        attacking = false;
    }






}
