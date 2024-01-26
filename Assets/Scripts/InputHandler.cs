using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct KeyInput
{
    public KeyCode keyCode;
    public bool isDown;
}

[Serializable]
public struct KeyBinding
{
    public string name;
    public KeyInput[] keyInputs;
    public UnityEvent onTriggered;
}

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    [SerializeField] private KeyBinding[] keyBindings;

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        for (int i = 0; i < keyBindings.Length; i++)
        {
            for (int j = 0; j < keyBindings[i].keyInputs.Length; j++)
            {
                if (keyBindings[i].keyInputs[j].isDown && Input.GetKeyDown(keyBindings[i].keyInputs[j].keyCode))
                {
                    keyBindings[i].onTriggered.Invoke();
                    continue;
                }

                if (!keyBindings[i].keyInputs[j].isDown && Input.GetKeyUp(keyBindings[i].keyInputs[j].keyCode))
                {
                    keyBindings[i].onTriggered.Invoke();
                    continue;
                }
            }
        }
    }

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }
}
