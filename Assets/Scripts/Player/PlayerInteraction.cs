using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    public IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera cam;


    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
            RayHit();
         
    }

    void RayHit()
    {
        lastCheckTime = Time.time;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, maxCheckDistance, layerMask))
        {
            if(rayHit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = rayHit.collider.gameObject;
                curInteractable = rayHit.collider.GetComponent<IInteractable>();
                SetPromptText();
            }
        }
        else
        {
            curInteractGameObject = null;
            curInteractable =null;
            promptText.gameObject.SetActive(false); 
        }
            
    }

    void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteraction()
    {
        curInteractable.OnInteract();
        curInteractGameObject = null;
        curInteractable = null;
        promptText.gameObject.SetActive(false);
    }

}
