using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    
    public abstract void Hit(double HitTime);

    protected BaseNote(double Timing, NoteType notetype)
    {
        timing = Timing;
        type = notetype;
    }
}

public class NormalNote : BaseNote
{
    public NormalNote(double timing) : base(timing, NoteType.Normal) {}

    public override void Hit(double HitTime)
    {

    }
}

public class SliderNote : BaseNote
{
    public double endTiming;

    public SliderNote(double start, double end) : base(start, NoteType.Slider)
    {
        endTiming = end;
    }

    public override void Hit(double HitTime)
    {

    }

    public void Release(double ReleaseTime)
    {

    }
}

public class MashNote : BaseNote
{
    public double endTiming;

    public MashNote(double start, double end) : base(start, NoteType.Mash)
    {
        endTiming = end;
    }

    public override void Hit(double HitTime)
    {

    }
}
