using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Song : MonoBehaviour
{
    public static Song Instance;
    public AudioSource audioSource;
    public float songDelayInSeconds;
    public int inputDelayInMiliseconds;
    public float noteTime;
    public float noteSpawnX;
    public float noteDespawnX;
    public float noteTapX;
    public List<NotesData> NotesData;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ReadSongAndMetadata();
    }

    IEnumerator LoadAudioAndPlay(string audioFilePath)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + audioFilePath, AudioType.OGGVORBIS);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);

            if (audioClip != null)
            {
                audioSource.clip = audioClip;
                Invoke(nameof(PlayAudio), songDelayInSeconds);
            }
            else
            {
                Debug.LogError("Audio clip is null!");
            }
        }
        else
        {
            Debug.LogError($"Failed to load audio: {www.error}");
        }
    }

    private void ReadSongAndMetadata()
    {
        NotesData = NoteTest.GenerateNotes();
        string path = Path.Combine(Application.dataPath, "Audio", "sample.ogg");
        StartCoroutine(LoadAudioAndPlay(path));
    }

    void PlayAudio()
    {
        audioSource.Play();
    }

    public static double GetAudioSourceTime() 
    {
        return Instance.audioSource.timeSamples / (double)Instance.audioSource.clip.frequency;
    }

}
