using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EditorSongSelectionManager : MonoBehaviour
{
    static public EditorSongSelectionManager instance;

    private SongInfo selectedSong;
    public UnityEvent<SongInfo> onSelectionChanged = new();
    public UnityEvent<SongInfo> onSelectionModified = new();

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public SongInfo ChangeSelection(SongInfo newSelection)
    {
        selectedSong = newSelection;
        GameData.songInfo = selectedSong;
        onSelectionChanged.Invoke(selectedSong);

        return selectedSong;
    }

    public SongInfo GetSelectedSong()
    {
        return selectedSong;
    }
}
