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
    public NotesData noteData;

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

        Debug.Log(result.score); // Add score here

        noteData.onHit.Invoke(result);

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
    private int ticks;

    public SliderNote(double start, double end, int Lane) : base(start, NoteType.Slider, Lane)
    {
        endTiming = end;
        ticks = 0;
    }

    public void SliderTick(double currentTime)
    {
        if (currentTime <= timing || currentTime >= endTiming) return;

        int newTicks = (int) Math.Floor(currentTime - timing / 1);

        if (newTicks > ticks) // Change tick distance from 1 later
        {
            Grade tickGrade = Threshold.instance.GetSpecialGrade("SliderTick");

            Debug.Log(tickGrade.score * (newTicks - ticks)); // Add score here

            ticks = newTicks;
        }
    }

    public void CheckMissStart(double currentTime)
    {
        if (currentTime < timing || Threshold.instance.GetGrade(currentTime - timing).score != 0) return;

        Debug.Log("Miss!"); // Register miss here

        ProcessInput.instance.inputEvent.RemoveListener(Press);
        Timeline.instance.updateEvent.RemoveListener(CheckMissStart);
        
        noteLocked = false;
    }

    public void CheckMissEnd(double currentTime)
    {
        if (currentTime < endTiming || Threshold.instance.GetGrade(currentTime - endTiming).score != 0) return;

        Debug.Log("Miss!"); // Register miss here

        ProcessInput.instance.inputEvent.RemoveListener(Release);
        Timeline.instance.updateEvent.RemoveListener(CheckMissEnd);
        Timeline.instance.updateEvent.RemoveListener(SliderTick);
    }

    public void Press((int lane, bool down) input)
    {
        if (input.lane != this.lane || !input.down) return;

        double currentTime = Timeline.instance.currentTime;

        Grade result = Threshold.instance.GetGrade(currentTime - timing);

        if (result.score == 0) return;

        Debug.Log(result.score); // Add score here

        noteData.onHit.Invoke(result);

        ProcessInput.instance.inputEvent.RemoveListener(Press);
        ProcessInput.instance.inputEvent.AddListener(Release);
        Timeline.instance.updateEvent.RemoveListener(CheckMissStart);
        Timeline.instance.updateEvent.AddListener(SliderTick);
        Timeline.instance.updateEvent.AddListener(CheckMissEnd);

        noteLocked = false;
    }

    public void Release((int lane, bool down) input)
    {
        if (input.lane != this.lane || input.down) return;

        double currentTime = Timeline.instance.currentTime;

        Grade result = Threshold.instance.GetGrade(currentTime - endTiming);

        Debug.Log(result.score); // Add score here

        noteData.onHit.Invoke(result);

        ProcessInput.instance.inputEvent.RemoveListener(Release);
        Timeline.instance.updateEvent.RemoveListener(SliderTick);
        Timeline.instance.updateEvent.RemoveListener(CheckMissEnd);
    }

    public override void LockNote()
    {
        noteLocked = true;
        ProcessInput.instance.inputEvent.AddListener(Press);
        Timeline.instance.updateEvent.AddListener(CheckMissStart);
    }
}

public class MashNote : BaseNote
{
    public double endTiming;

    public MashNote(double start, double end, int Lane) : base(start, NoteType.Mash, Lane)
    {
        endTiming = end;
    }

    public void Start(double currentTime)
    {
        if (currentTime < timing) return;

        Timeline.instance.updateEvent.RemoveListener(Start);
        Timeline.instance.updateEvent.AddListener(End);
        ProcessInput.instance.inputEvent.AddListener(Hit);

        noteLocked = false;
    }

    public void End(double currentTime)
    {
        if (currentTime < endTiming) return;

        Timeline.instance.updateEvent.RemoveListener(End);
        ProcessInput.instance.inputEvent.RemoveListener(Hit);
    }

    public void Hit((int lane, bool down) input)
    {
        if (input.lane != lane || !input.down) return;

        Grade mashGrade = Threshold.instance.GetSpecialGrade("MashGrade");

        Debug.Log(mashGrade.score); // Add score here

        noteData.onHit.Invoke(mashGrade);
    }

    public override void LockNote()
    {
        noteLocked = true;
        Timeline.instance.updateEvent.AddListener(Start);
    }
}
