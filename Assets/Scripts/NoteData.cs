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
    // Event for commmunicating between different systems of the same note (timing and rendering)
    [NonSerialized] public UnityEvent<Grade> onHit;

    [NonSerialized] public Grade resultGrade;

    public NoteData()
    {
        onHit = new();
        resultGrade = Grade.Null;
    }
}
