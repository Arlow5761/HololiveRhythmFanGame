using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// Singleton class that handles playing the song during gameplay
public class Song : MonoBehaviour
{
    public static Song Instance;
    public AudioSource audioSource;

    // Level data
    public float songDelayInSeconds;
    public int inputDelayInMiliseconds;
    public double noteTime;
    public double tickSpeed;
    public int baseDamage;
    public int baseHeal;
    public int baseFeverIncrease;

    // Notes data
    public List<NoteData> NotesData;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        audioSource = AudioSystem.instance.GetAudio("music", "music");
        ReadSongAndMetadata();
    }

    void FetchAudioAndPlay(string audioFilePath)
    {
        void OnAudioLoaded(string path, AudioClip audioClip)
        {
            if (path == audioFilePath)
            {
                audioSource.clip = audioClip;
                Invoke(nameof(PlayAudio), songDelayInSeconds);
                AudioLoader.onAudioLoaded.RemoveListener(OnAudioLoaded);
            }
        }

        AudioLoader.onAudioLoaded.AddListener(OnAudioLoaded);
        StartCoroutine(AudioLoader.LoadAudioFromFile(audioFilePath));
    }

    [Obsolete]
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
        LevelData levelData = NotesReader.ReadLevelDataFromFile(Path.Combine(Application.dataPath, GameData.selectedDifficulty.notePath));

        NotesData = levelData.Notes.ToList();
        noteTime = levelData.NoteSpeed;
        tickSpeed = levelData.TickSpeed;
        baseDamage = levelData.BaseDamage;
        baseHeal = levelData.BaseHeal;
        baseFeverIncrease = levelData.BaseFeverIncrease;

        Threshold.instance.grades = levelData.NormalThresholds;
        Threshold.instance.specialGrades = levelData.SpecialThresholds;
        Threshold.instance.SortGrades();

        string path = Path.Combine(Application.dataPath, GameData.songInfo.metadata.songPath);
        FetchAudioAndPlay(path);
    }

    void PlayAudio()
    {
        audioSource.Play();
        Timeline.instance.Run();
    }

    public static double GetAudioSourceTime() 
    {
        if (Instance.audioSource.clip == null) return 0;
        return Instance.audioSource.timeSamples / (double)Instance.audioSource.clip.frequency;
    }

}
