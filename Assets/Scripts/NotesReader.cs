using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class NotesReader
{
    public static List<NotesData> ReadNotesFromFile(string path)
    {
        string json = File.ReadAllText(path);
        
        List<NotesSerialData> rawData = JsonParser.OpenArray<NotesSerialData>(json).ToList();
        List<NotesData> data = new(rawData.Count);

        rawData.ForEach( serialNote => {
            data.Add(new() {
                TimestampStart = serialNote.TimestampStart,
                TimestampEnd = serialNote.TimestampEnd,
                NoteId = serialNote.NoteId,
                RowNumber = serialNote.RowNumber,
                NoteType = serialNote.NoteType
            });
        });

        data.ForEach(note => {

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

        return data;
    }
}
