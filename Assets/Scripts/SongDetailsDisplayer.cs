using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongDetailsDisplayer : MonoBehaviour
{
    public SongData displayedSong;

    public TextMeshProUGUI songNameField;
    public TextMeshProUGUI songArtistField;
    public TextMeshProUGUI songBPMField;

    public void ChangeSong(SongData newSong)
    {
        displayedSong = newSong;
        songNameField.SetText(displayedSong.metadata.songName);
        songArtistField.SetText("By " + displayedSong.metadata.artist);
        songBPMField.SetText("BPM : " + displayedSong.metadata.bpm);
    }
}
