using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Scene.LoadScene("SampleScene");
        }
    }
}
