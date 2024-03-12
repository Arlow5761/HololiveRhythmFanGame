using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class EditorSongNameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField songNameField;

    private SongInfo songInfo;

    void Start()
    {
        songInfo = EditorSongSelectionManager.instance.GetSelectedSong();
    }

    public void OnSelectionChanged(SongInfo newSongInfo)
    {
        songInfo = newSongInfo;
        songNameField.SetTextWithoutNotify(songInfo.metadata.songName);
    }

    public void ChangeSongName(string newSongName)
    {
        songInfo.metadata.songName = newSongName;
        EditorSongSelectionManager.instance.onSelectionModified.Invoke(songInfo);
    }
}
