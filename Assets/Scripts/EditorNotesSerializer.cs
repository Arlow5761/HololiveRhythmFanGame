using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorNotesSerializer : MonoBehaviour
{
    public static EditorNotesSerializer instance;

    public bool hasUnsavedData;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (instance != this && instance != null) return;

        instance = this;
    }

    public void SerializeLevelData()
    {
        LevelData dataToSerialize = new()
        {
            Notes = Song.Instance.NotesData.ToArray(),
            NoteSpeed = Song.Instance.noteTime,
            TickSpeed = Song.Instance.tickSpeed,
            BaseDamage = Song.Instance.baseDamage,
            BaseHeal = Song.Instance.baseHeal,
            BaseFeverIncrease = Song.Instance.baseFeverIncrease,
            NormalThresholds = Threshold.instance.grades,
            SpecialThresholds = Threshold.instance.specialGrades
        };

        string serializedData = JsonParser.WriteObject(dataToSerialize);
        string levelPath = Path.Combine(Application.dataPath, GameData.selectedDifficulty.notePath);
        File.WriteAllText(levelPath, serializedData);

        hasUnsavedData = false;
    }
}
