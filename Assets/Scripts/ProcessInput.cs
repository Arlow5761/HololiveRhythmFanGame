using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public class InputEvent : UnityEvent<(int lane, bool up)> {}

public class ProcessInput : MonoBehaviour
{
    public static ProcessInput instance;
    public Queue<(int lane, bool down)> inputQueue = new Queue<(int lane, bool down)>();
    public InputEvent inputEvent = new InputEvent();

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        Timeline.instance.updateEvent.AddListener(PushInputs);
    }

    void Update()
    {
        GetInputs();
    }

    public void Initialize()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void PushInputs(double _)
    {
        while (inputQueue.Count > 0)
        {
            inputEvent.Invoke(inputQueue.Dequeue());
        }
    }

    public void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.D)) inputQueue.Enqueue((0, true));
        if (Input.GetKeyDown(KeyCode.F)) inputQueue.Enqueue((0, true));
        if (Input.GetKeyDown(KeyCode.J)) inputQueue.Enqueue((1, true));
        if (Input.GetKeyDown(KeyCode.K)) inputQueue.Enqueue((1, true));
        if (Input.GetKeyUp(KeyCode.D)) inputQueue.Enqueue((0, false));
        if (Input.GetKeyUp(KeyCode.F)) inputQueue.Enqueue((0, false));
        if (Input.GetKeyUp(KeyCode.J)) inputQueue.Enqueue((1, false));
        if (Input.GetKeyUp(KeyCode.K)) inputQueue.Enqueue((1, false));
    }
}
