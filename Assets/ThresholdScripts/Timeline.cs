using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [Serializable] public struct HitObject
    {
        public enum HitObjectType
        {
            Press,
            SliderStart,
            SliderEnd,
            BeatdownStart,
            BeatdownEnd
        }

        public HitObjectType Type;
        public double Timepoint;

        public int StartingPoint; // Used by end points for sliders and beatdowns
        public bool SliderStarted; // Used to determine whether a slider start was hit or not
    }

    [Serializable] public struct ObjectStream
    {
        public HitObject[] HitObjects;
        public int Pointer;
    }

    public ObjectStream[] Lanes;
    public double CurrentTime;
    public Threshold LevelThreshold;
    public ProcessInput InputProcess;

    public ref HitObject GetCurrentHitObject(int Lane)
    {
        return ref GetHitObjectAt(Lane, Lanes[Lane].Pointer);
    }

    public ref HitObject GetHitObjectAt(int Lane, int Pointer)
    {
        return ref Lanes[Lane].HitObjects[Pointer];
    }

    public void ShiftPointer(int Lane, int Increment = 1)
    {
        Lanes[Lane].Pointer += Increment;
    }

    public void UpdateTime(double NewTime)
    {
        CurrentTime = NewTime;

        for (int i = 0; i < Lanes.Length; i++)
        {
            while (GetCurrentHitObject(i).Timepoint < CurrentTime - LevelThreshold.GetMaximumError())
            {
                InputProcess.Miss(i);
                ShiftPointer(i);
            }
        }
    }
}
