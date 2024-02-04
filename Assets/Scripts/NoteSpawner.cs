using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public struct NoteObject
{
    public string name;
    public GameObject gameObject;
}

public class NoteSpawner : MonoBehaviour
{
    public static NoteSpawner instance;

    public float hitPos;
    public float spawnDist;
    public NoteObject[] notes;

    private int[] current = {0, 0};

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        Timeline.instance.updateEvent.AddListener(OnUpdate);
    }

    public void Initialize()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnUpdate(double currentTime)
    {
        for (int i = 0; i < Timeline.instance.lanes.Length; i++)
        {
            if (current[i] >= Timeline.instance.lanes[i].stream.Length) continue;

            BaseNote note = Timeline.instance.lanes[i].stream[current[i]];

            while (note.timing - currentTime <= 2)
            {
                Spawn(note, note.lane);

                if (++current[i] >= Timeline.instance.lanes[i].stream.Length) break;

                note = Timeline.instance.lanes[i].stream[current[i]];
            }
        }
    }

    public void Spawn(BaseNote note, int lane)
    {
        // Make note movement be handled by the notes themselves later

        string type = "";

        switch (note.type)
        {
            case NoteType.Normal:
                type = "Normal";
            break;
            case NoteType.Slider:
                type = "Slider";
            break;
            case NoteType.Mash:
                type = "Mash";
            break;
        }

        GameObject notePrefab = Array.Find(notes, note => note.name == type).gameObject;

        GameObject noteObject = Instantiate(notePrefab, gameObject.transform);
        noteObject.transform.position = new Vector3(hitPos + spawnDist, -1 + 2 * lane, 1);

        void MoveNote(double currentTime)
        {
            noteObject.transform.position = Vector3.LerpUnclamped(new Vector3(hitPos, -1 + 2 * lane, 1), new Vector3(hitPos + spawnDist, -1 + 2 * lane, 1), (float) (note.timing - currentTime) / 2);
        }

        Timeline.instance.updateEvent.AddListener(MoveNote);
    }
}
