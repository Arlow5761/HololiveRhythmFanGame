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
            string validSongName = songInfo.metadata.songName;
            foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
            {
                validSongName = validSongName.Replace(invalidCharacter, '_');
            }
            Debug.Log(validSongName);

            if (songInfo.metadata.coverPath != null) File.Delete(Path.Combine(Application.dataPath, songInfo.metadata.songPath));
            File.Copy(newAudioPath, Path.Combine(Application.dataPath, "Songs", validSongName, "audio" + Path.GetExtension(newAudioPath)));

            songInfo.metadata.coverPath = Path.Combine("Songs", validSongName, "background" + Path.GetExtension(newAudioPath));
        }));
        
        EditorSongSelectionManager.instance.onSelectionModified.Invoke(songInfo);
    }
}
