using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public enum NoteType
{
    Normal,
    Slider,
    Mash,
}

public abstract class BaseNote
{
    public double timing;
    public NoteType type;
    public bool noteLocked;
    public int lane;

    public abstract void LockNote();

    protected BaseNote(double Timing, NoteType notetype, int Lane)
    {
        timing = Timing;
        type = notetype;
        lane = Lane;
    }
}

public class NormalNote : BaseNote
{
    public NormalNote(double timing, int Lane) : base(timing, NoteType.Normal, Lane) {}

    public void CheckForMiss(double currentTime)
    {
        if (currentTime < timing || Threshold.instance.GetGrade(currentTime - timing).score != 0) return;

        Debug.Log("Miss!"); // Register miss here

        CleanUp();
    }

    public void Hit((int lane, bool down) input)
    {
        if (input.lane != this.lane || !input.down) return;

        double currentTime = Timeline.instance.currentTime;

        Grade result = Threshold.instance.GetGrade(currentTime - timing);

        if (result.score == 0) return;

        Debug.Log(result.score);// Add score here

        CleanUp();
    }

    private void CleanUp()
    {
        Timeline.instance.updateEvent.RemoveListener(CheckForMiss);
        ProcessInput.instance.inputEvent.RemoveListener(Hit);
        noteLocked = false;
    }

    public override void LockNote()
    {
        noteLocked = true;
        ProcessInput.instance.inputEvent.AddListener(Hit);
        Timeline.instance.updateEvent.AddListener(CheckForMiss);
    }
}

public class SliderNote : BaseNote
{
    public double endTiming;

    public SliderNote(double start, double end, int Lane) : base(start, NoteType.Slider, Lane)
    {
        endTiming = end;
    }

    public void Hit(double HitTime)
    {

    }

    public void Release(double ReleaseTime)
    {

    }

    public override void LockNote()
    {
        noteLocked = true;
    }
}

public class MashNote : BaseNote
{
    public double endTiming;

    public MashNote(double start, double end, int Lane) : base(start, NoteType.Mash, Lane)
    {
        endTiming = end;
    }

    public void Hit(double HitTime)
    {

    }

    public override void LockNote()
    {
        noteLocked = true;
    }
}
