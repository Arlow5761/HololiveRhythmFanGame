using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Lane : MonoBehaviour
{
    readonly List<Note> notes = new();

    int spawnIndex = 0;
    readonly int inputIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    private GameObject LoadPrefabFromFile(string filename)
    {
        try {
            Debug.Log("Trying to load LevelPrefab from file ("+filename+ ")...");
            Debug.Log(Path.Combine("file://", Application.dataPath, "Preftabs", filename + ".prefab"));
            GameObject loadedObject = (GameObject)AssetDatabase.LoadAssetAtPath(Path.Combine("Assets/Preftabs", filename + ".prefab"),typeof(GameObject));
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
            0 => new Vector3(Song.Instance.noteSpawnX, 3f, 0),
            1 => new Vector3(Song.Instance.noteSpawnX, -3f, 0),
            _ => new Vector3(Song.Instance.noteSpawnX, 0, 0),
        };
        return Instantiate(preftab, position, new Quaternion(0, 0, 0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (spawnIndex < Song.Instance.NotesData.Count) {
            if (Song.GetAudioSourceTime() >= Song.Instance.NotesData[spawnIndex].TimestampStart - Song.Instance.noteTime) {
                GameObject note = SpawnNote(Song.Instance.NotesData[spawnIndex].NoteId, Song.Instance.NotesData[spawnIndex].RowNumber);
                notes.Add(note.AddComponent<Note>());
                spawnIndex++;
                Debug.Log("Note " + spawnIndex + " : " + note.transform.position.x + " " + note.transform.position.y);
                note.GetComponent<Note>().index = spawnIndex;
            }
        }

        // Action Hit
        if (inputIndex < Song.Instance.NotesData.Count) {
            
        }
    }
}
