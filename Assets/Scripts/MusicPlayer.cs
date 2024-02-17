using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Class to handle the music player in song select
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    public SongInfo displayedSong;
    public bool isPlaying = true;
    public UnityEvent<AudioClip> onMusicChanged;

    [SerializeField] private Slider musicProgressSlider;
    [SerializeField] private TextMeshProUGUI musicTimeStamp;
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;

    [SerializeField] private Sprite playingNormal;
    [SerializeField] private Sprite playingSelected;
    [SerializeField] private Sprite pausedNormal;
    [SerializeField] private Sprite pausedSelected;

    private AudioSource musicSource;

    public void ChangeMusic(SongInfo newSong)
    {
        if (musicSource == null) GetMusicSource();

        displayedSong = newSong;
        musicSource.Stop();

        void OnMusicLoaded(string path, AudioClip audioClip)
        {
            if (path == Path.Combine(Application.dataPath, displayedSong.metadata.songPath))
            {
                musicSource.clip = audioClip;
                PlayMusic();
                onMusicChanged.Invoke(audioClip);
                AudioLoader.onAudioLoaded.RemoveListener(OnMusicLoaded);
            }
        }

        AudioLoader.onAudioLoaded.AddListener(OnMusicLoaded);
        StartCoroutine(AudioLoader.LoadAudioFromFile(Path.Combine(Application.dataPath, newSong.metadata.songPath)));
    }

    public void PlayMusic()
    {
        button.interactable = false;
        button.spriteState = new SpriteState() {
            pressedSprite = pausedSelected,
            highlightedSprite = pausedSelected,
            disabledSprite = pausedSelected
            };
        buttonImage.sprite = pausedNormal;
        button.interactable = true;

        isPlaying = true;
        musicSource.Play();
    }

    public void PauseMusic()
    {
        button.interactable = false;
        button.spriteState = new SpriteState() {
            pressedSprite = playingSelected,
            highlightedSprite = playingSelected,
            disabledSprite = playingSelected
            };
        buttonImage.sprite = playingNormal;
        button.interactable = true;

        isPlaying = false;
        musicSource.Pause();
    }

    public void UnpauseMusic()
    {
        button.interactable = false;
        button.spriteState = new SpriteState() {
            pressedSprite = pausedSelected,
            highlightedSprite = pausedSelected,
            disabledSprite = pausedSelected
            };
        buttonImage.sprite = pausedNormal;
        button.interactable = true;

        isPlaying = true;
        musicSource.UnPause();
    }

    public void ToggleMusic()
    {
        if (isPlaying)
        {
            PauseMusic();
        }
        else
        {
            UnpauseMusic();
        }
    }

    public void UpdateMusicTimePosition(float value)
    {
        if (musicSource.clip == null) return;

        float clipLength = musicSource.clip.length;

        musicSource.time = clipLength * value;
    }
    
    void Update()
    {
        UpdateBarLength();
        UpdateMusicTimeStamp();
    }

    private void UpdateBarLength()
    {
        if (musicSource.clip == null) return;

        float currentTime = musicSource.time;
        float clipLength = musicSource.clip.length;

        musicProgressSlider.SetValueWithoutNotify(currentTime/clipLength);
    }

    private void UpdateMusicTimeStamp()
    {
        if (musicSource.clip == null) return;

        float currentTime = musicSource.time;

        int seconds = (int) (currentTime % 60);
        int minutes = (int) (currentTime / 60 % 60);
        int hours = (int) (currentTime / 360 % 60);

        string timeStamp = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        musicTimeStamp.SetText(timeStamp);
    }

    public void GetMusicSource()
    {
        musicSource = AudioSystem.instance.GetAudio("music", "music");
    }

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
