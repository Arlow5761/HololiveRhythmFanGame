using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorLane : MonoBehaviour
{
    private List<NoteRender> renderedNotes = new();
    [SerializeField] private Transform notesParent;

    // Start is called before the first frame update
    void Start()
    {
    }

    private GameObject LoadPrefabFromFile(string filename)
    {
        try {
//            Debug.Log("Trying to load LevelPrefab from file ("+filename+ ")...");
//            Debug.Log(Path.Combine("file://", Application.dataPath, "Preftabs", filename + ".prefab"));
            GameObject loadedObject = Resources.Load<GameObject>(filename);
            return loadedObject;
        } catch (IOException e) {
            Debug.LogError("Error loading LevelPrefab from file (" + filename + "): " + e.Message);
            return null;
        }
    }

    GameObject SpawnNote(int index, int rowNumber) {
        GameObject preftab = LoadPrefabFromFile("Note" + index);
        Vector3 position = rowNumber switch
        {
            0 => new Vector3(GameplayLayout.noteSpawnX, GameplayLayout.groundLaneY, 0),
            1 => new Vector3(GameplayLayout.noteSpawnX, GameplayLayout.airLaneY, 0),
            _ => new Vector3(GameplayLayout.noteSpawnX, 0, 0),
        };
        return Instantiate(preftab, position, new Quaternion(0, 0, 0, 1), notesParent);
    }

    // Update is called once per frame
    void Update()
    {
        // Clean up unrendered notes
        for (int index = 0; index < renderedNotes.Count; index++)
        {
            if (renderedNotes[index] == null)
            {
                renderedNotes.RemoveAt(index);
                index--;
            }
        }

        for (int spawnIndex = 0; spawnIndex < Song.Instance.NotesData.Count; spawnIndex++)
        {
            bool alreadySpawned = false;
            renderedNotes.ForEach(renderNote => {
                if (renderNote.noteData == Song.Instance.NotesData[spawnIndex]) alreadySpawned = true;
            });

            if (alreadySpawned) continue;

            if (Song.GetAudioSourceTime() <= Song.Instance.NotesData[spawnIndex].TimestampStart + Song.Instance.noteTime && Song.GetAudioSourceTime() >= Song.Instance.NotesData[spawnIndex].TimestampStart - Song.Instance.noteTime)
            {
                GameObject note = SpawnNote(Song.Instance.NotesData[spawnIndex].NoteId, Song.Instance.NotesData[spawnIndex].RowNumber);
                
                NoteRender noteRender = note.GetComponent<NoteRender>();
                noteRender.index = spawnIndex;
                noteRender.noteData = Song.Instance.NotesData[spawnIndex];

                CircleCollider2D noteCollider = note.AddComponent<CircleCollider2D>();
                noteCollider.isTrigger = true;

                renderedNotes.Add(noteRender);
            }
            else if (Song.GetAudioSourceTime() <= Song.Instance.NotesData[spawnIndex].TimestampEnd + Song.Instance.noteTime && Song.GetAudioSourceTime() >= Song.Instance.NotesData[spawnIndex].TimestampEnd - Song.Instance.noteTime)
            {
                GameObject note = SpawnNote(Song.Instance.NotesData[spawnIndex].NoteId, Song.Instance.NotesData[spawnIndex].RowNumber);
                
                NoteRender noteRender = note.GetComponent<NoteRender>();
                noteRender.index = spawnIndex;
                noteRender.noteData = Song.Instance.NotesData[spawnIndex];

                renderedNotes.Add(noteRender);
            }
        }
    }
}
