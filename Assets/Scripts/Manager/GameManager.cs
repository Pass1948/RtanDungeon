using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    // Managers=========================
    private static CharacterManager characterManager;
    public static CharacterManager Character => characterManager;

    private void Awake()
    {
        if (instance != null) { Destroy(this); return; }
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
        GameObject characterObj = new GameObject("CharacterManager");
        characterObj.transform.SetParent(transform, false);
        characterManager = characterObj.AddComponent<CharacterManager>();
    }

}
