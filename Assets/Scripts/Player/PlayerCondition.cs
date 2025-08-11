using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageIbe
{
    void TakePhtsicalDamaged(int damage);
}


public class PlayerCondition : MonoBehaviour, IDamageIbe
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public int noHungerHealthDecay;

    public event Action onTakeDamage;

    private void Update()
    {
        hunger.Add(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(hunger.curValue < 0f)
        {
            health.Subtract(noHungerHealthDecay*Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }
    public void Heal(float amout)
    {
        health.Add(amout);
    }

    public void Eat(float amout)
    {
        health.Add(amout);
    }

    public void Die()
    {
        Debug.Log("Á×À½");
    }

    public void TakePhtsicalDamaged(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
}
