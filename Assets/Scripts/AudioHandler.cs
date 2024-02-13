using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

// Class that holds an AudioSource with an AudioClip
[Serializable]
public class AudioContainer
{
    public enum State
    {
        Stopped,
        Playing,
        Paused
    }

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
    private bool isFrozen;
    private State state;

    public void PlayOneShot()
    {
        if (isFrozen) return;
        audioSource.PlayOneShot(audioClip);
    }

    public void Play()
    {
        audioSource.Play();
        state = State.Playing;

        if (isFrozen)
        {
            audioSource.Pause();
        }
    }

    public void Stop()
    {
        audioSource.Stop();
        state = State.Stopped;
    }

    public void Pause()
    {
        state = State.Paused;
        if (isFrozen) return;
        audioSource.Pause();
    }

    public void Unpause()
    {
        state = State.Playing;
        if (isFrozen) return;
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

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;

        if (isFrozen && state == State.Playing)
        {
            audioSource.Pause();
        }
        else
        if (!isFrozen && state == State.Playing)
        {
            audioSource.UnPause();
        }
    }
}

// Singleton class that manages audio
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

    public AudioContainer GetSFX(string name)
    {
        return Array.Find(sfxClips, clip => clip.name == name);
    }

    public void SetFrozenAll(bool isFrozen)
    {
        SetFrozenMusic(isFrozen);
        SetFrozenSfx(isFrozen);
    }

    public void SetFrozenMusic(bool isFrozen)
    {
        musicClip.SetFrozen(true);
    }

    public void SetFrozenSfx(bool isFrozen)
    {
        for (int i = 0; i < sfxClips.Count(); i++)
        {
            sfxClips[i].SetFrozen(isFrozen);
        }
    }

    public void onMusicVolumeChanged(float newVolume)
    {
        musicClip.SetVolume(newVolume);
    }

    public void onSfxVolumeChanged(float newVolume)
    {
        for (int i = 0; i < sfxClips.Count(); i++)
        {
            sfxClips[i].SetVolume(newVolume);
        }
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
