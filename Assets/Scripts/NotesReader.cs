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
            BaseNote noteTiming;

            if (note.TimestampEnd != note.TimestampStart)
            {
                noteTiming = note.NoteType switch
                {
                    "Hold" => new SliderNote(note.TimestampStart, note.TimestampEnd, note.RowNumber),
                    "Mash" => new MashNote(note.TimestampStart, note.TimestampEnd, note.RowNumber),
                    _ => null
                };
            }
            else
            {
                noteTiming = new NormalNote(note.TimestampStart, note.RowNumber);
            }

            Timeline.instance.lanes[note.RowNumber].stream.Add(noteTiming);

            note.timingObject = noteTiming;
            noteTiming.noteData = note;
        });

        return data;
    }
}
