using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public class InputEvent : UnityEvent<(int lane, bool up)> {}

// This class is used to notify notes when the player is trying to hit them
public class ProcessInput : MonoBehaviour
{
    public static ProcessInput instance;

    // Used by notes for hit detection
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

    public void Initialize()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Broadcasts the polled inputs
    public void PushInputs(double _)
    {
        while (inputQueue.Count > 0)
        {
            inputEvent.Invoke(inputQueue.Dequeue());
        }
    }

    // Functions to add inputs to the queue
    public void PollInputDown(int lane)
    {
        PollInput(lane, true);
    }

    public void PollInputUp(int lane)
    {
        PollInput(lane, false);
    }

    public void PollInput(int lane, bool down)
    {
        inputQueue.Enqueue((lane, down));
    }

    // Inputs should be sent by the PlayerController instead of manually polling
    [Obsolete] public void GetInputs()
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
