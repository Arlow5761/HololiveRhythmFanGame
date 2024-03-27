using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditorSongBPMUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField songBPMField;

    private SongInfo songInfo;

    void Start()
    {
        songInfo = EditorSongSelectionManager.instance.GetSelectedSong();
    }

    public void OnSelectionChanged(SongInfo newSongInfo)
    {
        songInfo = newSongInfo;
        songBPMField.SetTextWithoutNotify(songInfo.metadata.bpm);
    }

    public void ChangeSongBPM(string newSongBPM)
    {
        songInfo.metadata.bpm = newSongBPM;
        EditorSongSelectionManager.instance.onSelectionModified.Invoke(songInfo);
    }
}
