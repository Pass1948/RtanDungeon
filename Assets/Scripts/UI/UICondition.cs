using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    private void Start()
    {
        GameManager.Character.Player.condition.uiCondition = this;
    }




}
