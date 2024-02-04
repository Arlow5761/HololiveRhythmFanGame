using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;

    public UnityEvent onSceneLoaded = new UnityEvent();
    public UnityEvent onSceneUnloaded = new UnityEvent();

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void LoadScene(string sceneName)
    {
        onSceneUnloaded.Invoke();
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        onSceneUnloaded.Invoke();
        SceneManager.LoadSceneAsync(sceneName);
    }

    void Awake()
    {
        Initialize();

        onSceneLoaded.Invoke();
    }
}
