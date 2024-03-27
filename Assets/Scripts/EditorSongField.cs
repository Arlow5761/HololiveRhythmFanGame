using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorSongField : MonoBehaviour
{
    private SongInfo songInfo;

    void Start()
    {
        songInfo = EditorSongSelectionManager.instance.GetSelectedSong();
    }

    public void OnSelectionChanged(SongInfo newSongInfo)
    {
        songInfo = newSongInfo;
    }

    public void OnSongChanged()
    {
        StartCoroutine(FilesPanel.OpenFilePanel("Choose New Music Audio", Application.dataPath, newAudioPath => {
            songInfo.metadata.songPath = newAudioPath;
            EditorSongSelectionManager.instance.onSelectionModified.Invoke(songInfo);
        }));
    }
}
