using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class NoteSerialData // Used to save note data
{
    public double TimestampStart;
    public double TimestampEnd;
    public int NoteId;
    public int RowNumber;
    public string NoteType;
}

[Serializable]
public class NoteData : NoteSerialData // Used for in game note data
{
    [NonSerialized] public BaseNote timingObject;
    [NonSerialized] public NoteRender renderObject;

    [NonSerialized] public UnityEvent<Grade> onHit;

    public NoteData()
    {
        onHit = new();
    }
}
