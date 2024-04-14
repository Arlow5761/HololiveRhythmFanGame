using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesAudioPlayer : MonoBehaviour
{
    public static NotesAudioPlayer instance;

    public void Initialize()
    {
        if (instance == null) instance = this;
    }

    public void OnNormalNotePress(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Normal") return;

        AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", "mezzo");
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void OnHoldNotePress(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Hold") return;

        AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", "holdstart");
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void OnMashNotePress(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Mash") return;

        AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", "mezzo");
        audioSource.PlayOneShot(audioSource.clip);
    }

    void Awake()
    {
        Initialize();
    }
}
