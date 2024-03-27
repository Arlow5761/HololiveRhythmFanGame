using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EditorSongList : MonoBehaviour
{
    [SerializeField] private EditorSongItemUI songItemPrefab;
    [SerializeField] private Transform songItemParent;

    private List<EditorSongItemUI> songItems = new();

    void Start()
    {
        RefreshSongItems();
    }

    public void RefreshSongItems()
    {
        songItems.ForEach(songItem => {
            if (songItem != null) Destroy(songItem.gameObject);
        });

        songItems.Clear();

        foreach (SongInfo songInfo in SongsListReader.instance.songList)
        {
            EditorSongItemUI songItem = Instantiate(songItemPrefab, songItemParent);
            songItem.songInfo = songInfo;
            songItems.Add(songItem);
        }
    }

    public void AddSongItem()
    {
        List<SongInfo> tempList = SongsListReader.instance.songList.ToList(); // Change implementation in future to not recast whole list
        tempList.Add(new() { metadata = new() { songName = "New Song" } });
        SongsListReader.instance.songList = tempList.ToArray();

        

        RefreshSongItems();
    }
}
