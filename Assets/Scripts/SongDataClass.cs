using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SongData {
    public string SongName {get; set;}
    public string Artist {get; set;}
    public int Bpm {get; set;}
    public List<string> Difficulties {get; set;}
    public Dictionary<string, List<NotesData>> NotesData {get; set;}
}

[Serializable]
public class NotesSerialData // Used to save note data
{
    public int TimestampStart;
    public int TimestampEnd;
    public int NoteId;
    public int RowNumber;
    public string NoteType;
}

[Serializable]
public class NotesData : NotesSerialData // Used for in game note data
{
    public BaseNote timingObject;
    public NoteRender renderObject;

    public UnityEvent<Grade> onHit;

    public NotesData()
    {
        onHit = new();
    }
}

[Obsolete] public static class NoteTest {
    private static readonly List<NotesData> NotesData = new();
    public static List<NotesData> GenerateNotes() {
        for (int i = 0; i < 40; i+=2) {
            List<int> laneNumbers = new();
            for (int j = 0; j < UnityEngine.Random.Range(1,2); j++) {
                int laneNumber;
                do {
                    laneNumber = UnityEngine.Random.Range(0,2);
                    if (laneNumbers.Contains(laneNumber)) {
                        laneNumber = -1;
                        continue;
                    }
                    NotesData.Add(new NotesData {
                        TimestampStart = i,
                        TimestampEnd = i,
                        NoteId = UnityEngine.Random.Range(0,2),
                        RowNumber = laneNumber,
                        NoteType = "Normal"
                    });
                    break;
                } while (laneNumber == -1);
                laneNumbers.Add(laneNumber);
            }
        }

        int upperLaneCount  = 0;
        int lowerLaneCount = 0;

        NotesData.ForEach(note => {
            if (note.RowNumber == 0)
            {
                upperLaneCount++;
            }
            else if (note.RowNumber == 1)
            {
                lowerLaneCount++;
            }

            if (note.TimestampEnd != note.TimestampStart)
            {
                // Later
            }
            else
            {
                NormalNote noteTiming = new(note.TimestampStart, 1 - note.RowNumber);
                Timeline.instance.lanes[1 - note.RowNumber].stream.Add(noteTiming);

                note.timingObject = noteTiming;
                noteTiming.noteData = note;
            }
        });

        return NotesData;
    }
}