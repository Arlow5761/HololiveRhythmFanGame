using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Class to handle the music player in song select
public class MusicPlayer : MonoBehaviour
{
    public SongInfo displayedSong;
    public bool isPlaying = true;

    [SerializeField] private Slider musicProgressSlider;
    [SerializeField] private TextMeshProUGUI musicTimeStamp;
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;

    [SerializeField] private Sprite playingNormal;
    [SerializeField] private Sprite playingSelected;
    [SerializeField] private Sprite pausedNormal;
    [SerializeField] private Sprite pausedSelected;

    public void ChangeMusic(SongInfo newSong)
    {
        displayedSong = newSong;
        PauseMusic();
        StartCoroutine(AudioHandler.instance.LoadMusicClip(displayedSong.metadata.songName, Application.dataPath + @"\" + displayedSong.metadata.songPath));
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
        AudioHandler.instance.PlayMusic();
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
        AudioHandler.instance.PauseMusic();
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
        AudioHandler.instance.UnpauseMusic();
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

    public void StartMusic(AudioContainer audioContainer)
    {
        if (audioContainer.name == displayedSong.metadata.songName)
        {
            PlayMusic();
        }
    }

    public void UpdateMusicTimePosition(float value)
    {
        if (AudioHandler.instance.musicClip.audioClip == null) return;

        float clipLength = AudioHandler.instance.musicClip.GetClipLength();

        AudioHandler.instance.musicClip.audioSource.time = clipLength * value;
    }
    
    void Update()
    {
        UpdateBarLength();
        UpdateMusicTimeStamp();
    }

    private void UpdateBarLength()
    {
        if (AudioHandler.instance.musicClip.audioClip == null) return;

        float currentTime = AudioHandler.instance.musicClip.GetTimePosition();
        float clipLength = AudioHandler.instance.musicClip.GetClipLength();

        musicProgressSlider.SetValueWithoutNotify(currentTime/clipLength);
    }

    private void UpdateMusicTimeStamp()
    {
        if (AudioHandler.instance.musicClip.audioClip == null) return;

        float currentTime = AudioHandler.instance.musicClip.GetTimePosition();

        int seconds = (int) (currentTime % 60);
        int minutes = (int) (currentTime / 60 % 60);
        int hours = (int) (currentTime / 360 % 60);

        string timeStamp = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        musicTimeStamp.SetText(timeStamp);
    }
}
