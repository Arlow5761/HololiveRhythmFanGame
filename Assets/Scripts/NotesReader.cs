using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore;

[Serializable]
public class LevelData
{
    public Grade[] NormalThresholds;
    public Grade[] SpecialThresholds;
    public double TickSpeed;
    public double NoteSpeed;
    public int BaseDamage;
    public NoteData[] Notes;
}

public static class NotesReader
{
    public static LevelData ReadLevelDataFromFile(string path)
    {
        string json = File.ReadAllText(path);
        
        LevelData rawData = JsonParser.OpenObject<LevelData>(json);

        foreach (NoteData note in rawData.Notes)
        {
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
                noteTiming = note.NoteType switch
                {
                    "Normal" => new NormalNote(note.TimestampStart, note.RowNumber),
                    "Heal" => new HealNote(note.TimestampStart, note.RowNumber),
                    "Obstacle" => new ObstacleNote(note.TimestampStart, note.RowNumber),
                    _ => null
                };
            }

            Timeline.instance.lanes[note.RowNumber].stream.Add(noteTiming);

            note.timingObject = noteTiming;
            noteTiming.noteData = note;
        };

        return rawData;
    }
}
