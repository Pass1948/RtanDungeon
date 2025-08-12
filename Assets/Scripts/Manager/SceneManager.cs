using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log($"{sceneName}");
        StartCoroutine(LoadingCoroutine(sceneName));
    }

    IEnumerator LoadingCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        Time.timeScale = 1f;
        Debug.Log($"로드 끝남");
        yield return new WaitForSeconds(0.5f);
    }
}
