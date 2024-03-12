using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditorSongArtistUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField songArtistField;

    private SongInfo songInfo;

    void Start()
    {
        songInfo = EditorSongSelectionManager.instance.GetSelectedSong();
    }

    public void OnSelectionChanged(SongInfo newSongInfo)
    {
        songInfo = newSongInfo;
        songArtistField.SetTextWithoutNotify(songInfo.metadata.artist);
    }

    public void ChangeSongArtist(string newSongArtist)
    {
        songInfo.metadata.artist = newSongArtist;
        EditorSongSelectionManager.instance.onSelectionModified.Invoke(songInfo);
    }
}
