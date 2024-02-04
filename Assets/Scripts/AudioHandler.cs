using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[Serializable]
public class AudioContainer
{
    public string name = "";
    public bool loops {get; private set;}
    public AudioClip audioClip
    {
        get
        {
            return _audioClip;
        }

        set
        {
            _audioClip = value;

            if (audioSource.isPlaying)
            {
                Stop();
            }

            audioSource.clip = _audioClip;
        }
    }
    public AudioSource audioSource = null;

    [SerializeField] private AudioClip _audioClip = null;

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Unpause()
    {
        audioSource.UnPause();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void CreateAudioSoure()
    {
        GameObject sourceObject = new GameObject(name, typeof(AudioSource));
        sourceObject.transform.parent = AudioHandler.instance.transform;
        sourceObject.name = name;

        audioSource = sourceObject.GetComponent<AudioSource>();
        audioSource.clip = _audioClip;
    }

    public void SetLoops(bool loop)
    {
        loops = loop;
        if (audioSource != null)
        {
            audioSource.loop = loops;
        }
    }

    public float GetClipLength()
    {
        return audioClip.length;
    }

    public float GetTimePosition()
    {
        return audioSource.time;
    }
}

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler instance;

    public AudioContainer musicClip;
    public AudioContainer[] sfxClips;
    public UnityEvent<AudioContainer> onMusicClipChanged;

    void Awake()
    {
        Initialize();

        musicClip.CreateAudioSoure();
        musicClip.SetLoops(true);

        for (int i = 0; i < sfxClips.Length; i++)
        {
            sfxClips[i].CreateAudioSoure();
        }
    }

    void Start()
    {
    }

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void PlayMusic()
    {
        musicClip.Play();
    }

    public void PauseMusic()
    {
        musicClip.Pause();
    }

    public void UnpauseMusic()
    {
        musicClip.Unpause();
    }

    public void PlaySfx(string name)
    {
        Array.Find(sfxClips, clip => clip.name == name).Play();
    }

    public IEnumerator LoadMusicClip(string name, string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                ((DownloadHandlerAudioClip) www.downloadHandler).streamAudio = true;
                AudioClip audioClip = ((DownloadHandlerAudioClip) www.downloadHandler).audioClip;

                if (audioClip == null)
                {
                    Debug.LogError("Audio clip is null.");
                }
                else
                {
                    musicClip.name = name;
                    musicClip.audioClip = audioClip;
                    musicClip.audioClip.name = name;
                    onMusicClipChanged.Invoke(musicClip);
                }
            }
            else
            {
                Debug.LogError("Error loading clip: \"" + path + "\"");
            }
        }
    }
}
