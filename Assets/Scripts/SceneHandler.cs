using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Class that handles scene management
public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;

    public UnityEvent onSceneLoaded = new UnityEvent();
    public UnityEvent onSceneUnloaded = new UnityEvent();

    private int unloadBlockers;

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void LoadScene(string sceneName)
    {
        onSceneUnloaded.Invoke();
        StartCoroutine(Utility.WaitUntil(
            () => {return unloadBlockers <= 0;},
            () => {SceneManager.LoadScene(sceneName);}
        ));
    }

    public void LoadSceneAsync(string sceneName)
    {
        onSceneUnloaded.Invoke();

        StartCoroutine(Utility.WaitUntil(
            () => {return unloadBlockers <= 0;},
            () => {SceneManager.LoadSceneAsync(sceneName);}
        ));
    }

    public void OpenLoadingScene()
    {
        LoadingSceneManager.LoadLoadingScene();
    }

    public void CloseLoadingScene()
    {
        LoadingSceneManager.UnloadLoadingScene();
    }

    public void BlockUnload()
    {
        unloadBlockers++;
    }

    public void UnblockUnload()
    {
        unloadBlockers--;
    }

    void Awake()
    {
        Initialize();

        onSceneLoaded.Invoke();
    }

    public void Start()
    {
        if (!LoadingSceneManager.instance.loadingObject.activeSelf) return;
        CloseLoadingScene();
    }
}
