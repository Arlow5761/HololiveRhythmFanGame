using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public Song displayedSong;
    public bool isPlaying;

    public void ChangeMusic(Song newSong)
    {
        displayedSong = newSong;
        PauseMusic();
        StartCoroutine(AudioHandler.instance.LoadMusicClip(displayedSong.metadata.songName, Application.dataPath + @"\" + displayedSong.metadata.songPath));
    }

    public void PlayMusic()
    {
        AudioHandler.instance.PlayMusic();
    }

    public void PauseMusic()
    {
        isPlaying = false;
        AudioHandler.instance.PauseMusic();
    }

    public void UnpauseMusic()
    {
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
}
