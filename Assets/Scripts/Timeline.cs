using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public static class Utility
{
    static public IEnumerator Wait(float delay, Action function)
    {
        yield return new WaitForSeconds(delay);
        function();
    }

    static public IEnumerator WaitUntil(Func<bool> predicate, Action function)
    {
        yield return new WaitUntil(predicate);
        function();
    }
}

public class TimestampEvent : UnityEvent<double> {}

// Class that handles note timings on a single lane
[Serializable]
public class TimingLane
{
    public List<BaseNote> stream;
    public bool ended = false;

    private int current = 0;

    public TimingLane(int Buffer = 100)
    {
        current = 0;
        ended = false;
        stream = new();
    }

    public bool IsCurrentNoteLocked()
    {
        if (current == stream.Count)
        {
            ended = true;
            return true;
        }

        return stream[current].noteLocked;
    }

    public int LockNextNote()
    {
        if (++current == stream.Count)
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

// Class that handles timings on all lanes
public class Timeline : MonoBehaviour
{
    public static Timeline instance;
    public double currentTime;
    [SerializeField] public TimingLane[] lanes;
    public TimestampEvent updateEvent = new TimestampEvent();
    public UnityEvent onLevelEnded;

    private bool started = false;
    private bool finished = false;

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

        Array.Resize(ref lanes, 2);
        lanes[0] = new();
        lanes[1] = new();
    }

    public void Run()
    {
        started = true;
        
        if (lanes[0].stream.Count > 0)
        {
            lanes[0].stream[0].LockNote();
        }

        if (lanes[1].stream.Count > 0)
        {
            lanes[1].stream[0].LockNote();
        }
    }

    void Update()
    {
        if (!started) return;
        
        UpdateTime(Song.GetAudioSourceTime());
    }

    public void UpdateTime(double time)
    {
        currentTime = time;
        UpdateNotes();
        updateEvent.Invoke(currentTime);

        if (time > Song.Instance.NotesData.Last().TimestampEnd && !finished)
        {
            finished = true;
            StartCoroutine(Utility.Wait(4, () => {
                onLevelEnded.Invoke();
            }));
        }
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
