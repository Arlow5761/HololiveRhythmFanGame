using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using JetBrains.Annotations;

public enum NoteType
{
    Normal,
    Slider,
    Mash,
    Heal,
    Score,
    Obstacle
}

// Base class for all note timings
public abstract class BaseNote
{
    public double timing;
    public NoteType type;
    public bool noteLocked;
    public int lane;
    public NoteData noteData;

    public abstract void LockNote();

    protected BaseNote(double Timing, NoteType notetype, int Lane)
    {
        timing = Timing;
        type = notetype;
        lane = Lane;
    }
}

// Class for normal note timings
public class NormalNote : BaseNote
{
    public NormalNote(double timing, int Lane) : base(timing, NoteType.Normal, Lane) {}

    public void CheckForMiss(double currentTime)
    {
        if (currentTime < timing || Threshold.instance.GetGrade(currentTime - timing).score != 0) return;

        ScoreManager.instance.BreakCombo();

        //if (PlayerController.instance.GetLane() == lane)
        //{
        //    PlayerController.instance.Damage(Song.Instance.baseDamage);
        //}

        noteData.onHit.Invoke(Threshold.instance.GetSpecialGrade("Miss"));
        NotesManager.instance.onNoteMiss.Invoke(noteData, Threshold.instance.GetSpecialGrade("Miss"));
        NotesManager.instance.onNotePass.Invoke(noteData, Threshold.instance.GetSpecialGrade("Miss"));

        CleanUp();
    }

    public void Hit((int lane, bool down) input)
    {
        if (input.lane != this.lane || !input.down) return;

        double currentTime = Timeline.instance.currentTime;

        Grade result = Threshold.instance.GetGrade(currentTime - timing);

        if (result.score == 0) return;

        noteData.onHit.Invoke(result);
        NotesManager.instance.onNotePress.Invoke(noteData, result);
        NotesManager.instance.onGetGrade.Invoke(noteData, result);

        //AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", "mezzo");
        //audioSource.PlayOneShot(audioSource.clip);

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

// Class for slider note timings
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

        int newTicks = (int) Math.Floor((currentTime - timing) / Song.Instance.tickSpeed);

        if (newTicks > ticks)
        {
            Grade tickGrade = Threshold.instance.GetSpecialGrade("SliderTick");

            for (int i = 0; i < newTicks - ticks; i++)
            {
                NotesManager.instance.onGetGrade.Invoke(noteData, tickGrade);
            }

            ticks = newTicks;
        }
    }

    public void CheckMissStart(double currentTime)
    {
        if (currentTime < timing || Threshold.instance.GetGrade(currentTime - timing).score != 0) return;

        noteData.onHit.Invoke(Threshold.instance.GetSpecialGrade("Miss"));
        NotesManager.instance.onNoteMiss.Invoke(noteData, Threshold.instance.GetSpecialGrade("Miss"));

        ProcessInput.instance.inputEvent.RemoveListener(Press);
        Timeline.instance.updateEvent.RemoveListener(CheckMissStart);
        
        noteLocked = false;
    }

    public void CheckMissEnd(double currentTime)
    {
        if (currentTime < endTiming || Threshold.instance.GetGrade(currentTime - endTiming).score != 0) return;

        noteData.onHit.Invoke(Threshold.instance.GetSpecialGrade("Miss"));
        NotesManager.instance.onNoteMiss.Invoke(noteData, Threshold.instance.GetSpecialGrade("Miss"));
        NotesManager.instance.onNoteRelease.Invoke(noteData, Threshold.instance.GetSpecialGrade("Miss"));

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

        noteData.onHit.Invoke(result);
        NotesManager.instance.onNotePress.Invoke(noteData, result);
        NotesManager.instance.onGetGrade.Invoke(noteData, result);

        //AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", "holdstart");
        //audioSource.PlayOneShot(audioSource.clip);

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

        if (result.score != 0)
        {
            NotesManager.instance.onGetGrade.Invoke(noteData, result);
        }
        else
        {
            NotesManager.instance.onNoteMiss.Invoke(noteData, result);
        }

        noteData.onHit.Invoke(result);
        NotesManager.instance.onNoteRelease.Invoke(noteData, result);

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

// Class for mash note timings
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

        //ScoreManager.instance.IncrementCombo();
        //ScoreManager.instance.AddScoreWithCombo(mashGrade.score);
        //PlayerController.instance.IncreaseFever(Song.Instance.baseFeverIncrease / 3);

        noteData.onHit.Invoke(mashGrade);
        NotesManager.instance.onNotePress.Invoke(noteData, mashGrade);
        NotesManager.instance.onGetGrade.Invoke(noteData, mashGrade);
        
        //AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", "mezzo");
        //audioSource.PlayOneShot(audioSource.clip);
    }

    public override void LockNote()
    {
        noteLocked = true;
        Timeline.instance.updateEvent.AddListener(Start);
    }
}

// Class for heal note timings
public class HealNote : BaseNote
{
    public HealNote(double timing, int Lane) : base(timing, NoteType.Heal, Lane) {}

    public void CheckForCollision(double currentTime)
    {
        if (currentTime < timing) return;

        Grade healGrade =  Threshold.instance.GetSpecialGrade("Miss");

        //PlayerController.instance.Heal(Song.Instance.baseDamage);
        noteData.onHit.Invoke(healGrade);
        NotesManager.instance.onNoteHit.Invoke(noteData, healGrade);
        NotesManager.instance.onGetGrade.Invoke(noteData, healGrade);

        CleanUp();
    }

    private void CleanUp()
    {
        Timeline.instance.updateEvent.RemoveListener(CheckForCollision);
        noteLocked = false;
    }

    public override void LockNote()
    {
        noteLocked = false;
        Timeline.instance.updateEvent.AddListener(CheckForCollision);
    }
}

// Class for obstacle note timings
public class ObstacleNote : BaseNote
{
    public ObstacleNote(double timing, int Lane) : base(timing, NoteType.Obstacle, Lane) {}

    public void CheckForCollision(double currentTime)
    {
        if (currentTime < timing) return;

        Grade obstacleGrade = Threshold.instance.GetSpecialGrade("Miss");

        //ScoreManager.instance.BreakCombo();
        //PlayerController.instance.Damage(Song.Instance.baseDamage);
        noteData.onHit.Invoke(obstacleGrade);
        NotesManager.instance.onNoteHit.Invoke(noteData, obstacleGrade);
        NotesManager.instance.onGetGrade.Invoke(noteData, obstacleGrade);
        //Scores.grades["Miss"]++;

        CleanUp();
    }

    private void CleanUp()
    {
        Timeline.instance.updateEvent.RemoveListener(CheckForCollision);
        noteLocked = false;
    }

    public override void LockNote()
    {
        noteLocked = false;
        Timeline.instance.updateEvent.AddListener(CheckForCollision);
    }
}

// Class for score note timings
public class ScoreNote : BaseNote
{
    public ScoreNote(double timing, int Lane) : base(timing, NoteType.Score, Lane) {}

    public void CheckForCollision(double currentTime)
    {
        if (currentTime < timing) return;

        Grade grade = Threshold.instance.GetSpecialGrade("ScoreNote");

        //ScoreManager.instance.AddScoreWithCombo(grade.score); // Change the added score
        noteData.onHit.Invoke(grade);
        NotesManager.instance.onGetGrade.Invoke(noteData, grade);

        CleanUp();
    }

    private void CleanUp()
    {
        Timeline.instance.updateEvent.RemoveListener(CheckForCollision);
        noteLocked = false;
    }

    public override void LockNote()
    {
        noteLocked = false;
        Timeline.instance.updateEvent.AddListener(CheckForCollision);
    }
}
