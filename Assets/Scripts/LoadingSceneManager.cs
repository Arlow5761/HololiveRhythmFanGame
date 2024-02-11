using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static LoadingSceneManager instance;

    [SerializeField] private Animator loadingScreenAnimator;
    public GameObject loadingObject;

    public static void LoadLoadingScene()
    {
        if (instance == null) return;

        instance.Open();
    }

    public static void UnloadLoadingScene()
    {
        if (instance == null) return;
        instance.InternalUnloadLoadingScene();
    }

    private void InternalUnloadLoadingScene()
    {
        loadingScreenAnimator.SetTrigger("Exit");
    }

    public void Initialize()
    {
        if (instance != null && instance != this)
        {
            Destroy(loadingObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(loadingObject);
    }

    public void WaitForLoadingScreenExit()
    {
        Close();
    }

    public void WaitForLoadingScreenEntry()
    {
        SceneHandler.instance.UnblockUnload();
    }

    private void Open()
    {
        SceneHandler.instance.BlockUnload();
        loadingObject.SetActive(true);
        loadingScreenAnimator.Rebind();
        loadingScreenAnimator.Update(0f);
    }

    private void Close()
    {
        loadingObject.SetActive(false);
    }
}
