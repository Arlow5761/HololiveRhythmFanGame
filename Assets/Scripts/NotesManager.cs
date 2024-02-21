using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Class to manage notes and note relations, will be expanded
public class NotesManager : MonoBehaviour
{
    public static NotesManager instance;

    public List<NoteData> notesData;

    public UnityEvent<NoteData, Grade> onNotePress;
    public UnityEvent<NoteData, Grade> onNoteMiss;
    public UnityEvent<NoteData, Grade> onNoteRelease;
    public UnityEvent<NoteData, Grade> onNoteHit;
    public UnityEvent<NoteData, Grade> onNotePass;
    public UnityEvent<NoteData, Grade> onGetGrade;

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    void Awake()
    {
        Initialize();
    }
}
