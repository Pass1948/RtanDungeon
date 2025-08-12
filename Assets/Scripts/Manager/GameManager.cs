using System.Resources;
using UnityEngine;

[DefaultExecutionOrder(-100)]       // GameManager가 다른 Script보다 먼저 호출되게 설정
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    // Managers=========================
    private static CharacterManager characterManager;
    public static CharacterManager Character => characterManager;

    private static ResourceManager resourceManager;
    public static ResourceManager Resource => resourceManager;

    private static PoolManager poolManager;
    public static PoolManager Pool => poolManager;

    private static SceneManager sceneManager;
    public static SceneManager Scene => sceneManager;


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

        GameObject resourceObj = new GameObject("ResourceManager");
        resourceObj.transform.SetParent(transform, false);
        resourceManager = resourceObj.AddComponent<ResourceManager>();

        GameObject poolObj = new GameObject("PoolManager");
        poolObj.transform.SetParent(transform, false);
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject sceneObj = new GameObject("SceneManager");
        sceneObj.transform.SetParent(transform, false);
        sceneManager = sceneObj.AddComponent<SceneManager>();
    }

}
