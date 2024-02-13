using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Class that handles displaying song details in the song select screen
public class SongDetailsDisplayer : MonoBehaviour
{
    public SongInfo displayedSong;

    public TextMeshProUGUI songNameField;
    public TextMeshProUGUI songArtistField;
    public TextMeshProUGUI songBPMField;
    public TextMeshProUGUI songLengthField;

    public void ChangeSong(SongInfo newSong)
    {
        displayedSong = newSong;
        songNameField.SetText(displayedSong.metadata.songName);
        songArtistField.SetText("By " + displayedSong.metadata.artist);
        songBPMField.SetText("BPM : " + displayedSong.metadata.bpm);
    }

    public void ChangeSongLength(AudioContainer newAudioContainer)
    {
        float clipLength = newAudioContainer.GetClipLength();

        int seconds = (int) (clipLength % 60);
        int minutes = (int) (clipLength / 60 % 60);
        int hours = (int) (clipLength / 360 % 60);

        string timeStamp;

        if (hours > 0)
        {
            timeStamp = string.Format("{0}h {1}m {2}s", hours, minutes, seconds);
        }
        if (minutes > 0)
        {
            timeStamp = string.Format("{0}m {1}s", minutes, seconds);
        }
        else
        {
            timeStamp = string.Format("{0}s", seconds);
        }

        songLengthField.SetText("Length: " + timeStamp);
    }
}
