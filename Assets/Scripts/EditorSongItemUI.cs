using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EditorSongItemUI : MonoBehaviour
{
    public SongInfo songInfo;

    private static EditorSongItemUI selectedSongItem;
    private bool onModifiedBounded = false;

    [SerializeField] private TextMeshProUGUI songNameLabel;

    void Start()
    {
        songNameLabel.SetText(songInfo.metadata.songName);
        
        if (!onModifiedBounded)
        {
            EditorSongSelectionManager.instance.onSelectionModified.AddListener((songInfo) => {OnModified();});
            onModifiedBounded = true;
        }
    }

    public static void OnModified()
    {
        selectedSongItem.songNameLabel.SetText(selectedSongItem.songInfo.metadata.songName);
    }

    public void OnSelect()
    {
        EditorSongSelectionManager.instance.ChangeSelection(songInfo);
        selectedSongItem = this;
    }

    public void OnDelete()
    {
        List<SongInfo> tempList = SongsListReader.instance.songList.ToList(); // Try to change implementation in future to not recast whole array
        tempList.Remove(songInfo);

        SongsListReader.instance.songList = tempList.ToArray();
        Destroy(gameObject);
    }
}
