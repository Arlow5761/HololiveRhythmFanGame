using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TimestampEvent : UnityEvent<double> {}

[Serializable]
public class Lane
{
    public BaseNote[] stream;
    public bool ended = false;

    private int current = 0;

    public Lane(int Buffer = 100)
    {
        current = 0;
        ended = false;
        Array.Resize(ref stream, Buffer);
    }

    public bool IsCurrentNoteLocked()
    {
        return stream[current].noteLocked;
    }

    public int LockNextNote()
    {
        if (++current == stream.Length)
        {
            ended = true;
        }
        else
        {
            stream[current].LockNote();
        }

        return current;
    }
}

public class Timeline : MonoBehaviour
{
    public static Timeline instance;
    public double currentTime;
    [SerializeField] public Lane[] lanes;
    public TimestampEvent updateEvent = new TimestampEvent();

    void Awake()
    {
        Initialize();
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

    void Start()
    {
        // Code below is for testing
        Array.Resize(ref lanes, 2);
        lanes[0] = new Lane(1);
        lanes[0].stream[0] = new NormalNote(5, 0);
        lanes[0].stream[0].LockNote();

        lanes[1] = new Lane(2);
        lanes[1].stream[0] = new NormalNote(6, 1);
        lanes[1].stream[1] = new NormalNote(7, 1);
        lanes[1].stream[0].LockNote();
    }

    void Update()
    {
        UpdateTime(currentTime += Time.deltaTime);
    }

    public void UpdateTime(double time)
    {
        currentTime = time;
        UpdateNotes();
        updateEvent.Invoke(currentTime);
    }

    public void UpdateNotes()
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            if (lanes[i].ended) continue;
            if (!lanes[i].IsCurrentNoteLocked()) lanes[i].LockNextNote();
        }
    }
}
