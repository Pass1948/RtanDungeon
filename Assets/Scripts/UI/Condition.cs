using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [SerializeField] float starValue;
    [SerializeField] float maxValue;
    public float curValue;
    public float passiveValue;
    [SerializeField] Image uiBar;


    private void Start()
    {
        curValue = starValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPerentage();
    }

    float GetPerentage()
    {
        return curValue/maxValue;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0f);
    }
}
