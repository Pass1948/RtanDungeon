using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }


    // Managers=========================
    private static CharacterManager characterManager;
    public static CharacterManager Character { get { return characterManager; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject characterObj = new GameObject();
        characterObj.name = "CharacterManager";
        characterObj.transform.parent = transform;
        characterManager = characterObj.AddComponent<CharacterManager>();

    }

}
