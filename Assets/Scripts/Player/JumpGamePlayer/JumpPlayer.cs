using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayer : MonoBehaviour
{
    public JumpPlayerCotroller controller;

    private void Awake()
    {
        GameManager.Character.JumpPlayer = this;
        controller = GetComponent<JumpPlayerCotroller>();
    }
}
