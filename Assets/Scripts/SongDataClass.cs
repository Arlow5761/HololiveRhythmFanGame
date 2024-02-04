using System.Collections.Generic;
using UnityEngine;

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
        return NotesData;
    }
}