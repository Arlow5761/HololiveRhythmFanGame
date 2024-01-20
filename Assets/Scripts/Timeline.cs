using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lane
{
    public BaseNote[] stream;

    private int pointer = 0; // used to speed up look up

    public Lane(int Buffer = 100)
    {
        Array.Resize(ref stream, Buffer);
    }

    public (int start, int end) GetIndexInTimeRange(double start, double end)
    {
        int first = pointer;
        int last = pointer + 1;

        bool firstFound = false;

        for (int i = pointer; i < stream.Length; i++)
        {
            if (stream[i].timing > start && !firstFound)
            {
                first = i;
                firstFound = true;
                pointer = first;
            }

            if (stream[i].timing > end)
            {
                last = i;
                return (first, last);
            }
        }

        last = stream.Length;
        return (first, last);
    }
}

public class Timeline : MonoBehaviour
{
    public double currentTime;
    public Lane[] lanes;

    private (int start, int end)[] activeWindow;

    public void UpdateTime(double time)
    {
        currentTime = time;

        for (int i = 0; i < lanes.Length; i++)
        {
            activeWindow[i] = lanes[i].GetIndexInTimeRange(currentTime, currentTime + 1);
        }
    }
}
