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



public class NotesData {
    public int TimestampStart {get; set;}
    public int TimestampEnd {get; set;}
    public int NoteId {get; set;}
    public int RowNumber {get; set;}
    public string NoteType {get; set;}

    public BaseNote timingObject;
    public Note renderObject;

    public UnityEvent<Grade> onHit;

    public NotesData()
    {
        onHit = new();
    }
}

public static class NoteTest {
    private static readonly List<NotesData> NotesData = new();
    public static List<NotesData> GenerateNotes() {
        for (int i = 0; i < 40; i+=2) {
            List<int> laneNumbers = new();
            for (int j = 0; j < Random.Range(1,2); j++) {
                int laneNumber;
                do {
                    laneNumber = Random.Range(0,2);
                    if (laneNumbers.Contains(laneNumber)) {
                        laneNumber = -1;
                        continue;
                    }
                    NotesData.Add(new NotesData {
                        TimestampStart = i,
                        TimestampEnd = i,
                        NoteId = Random.Range(0,2),
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