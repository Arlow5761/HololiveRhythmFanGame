using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EditorDifficultyItemUI : MonoBehaviour
{
    public SongInfo songInfo;
    public Difficulty difficulty;

    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField ratingField;

    void Start()
    {
        nameField.SetTextWithoutNotify(difficulty.name);
        ratingField.SetTextWithoutNotify(difficulty.rating.ToString());
    }

    public void ChangeName(string newName)
    {
        int difficultyIndex = 0;
        for (; difficultyIndex < songInfo.metadata.difficulties.Length; difficultyIndex++)
        {
            if (songInfo.metadata.difficulties[difficultyIndex].name == difficulty.name) break;
        }

        songInfo.metadata.difficulties[difficultyIndex].name = newName;
        difficulty.name = newName;
    }

    public void ChangeRating(string newRating)
    {
        int difficultyIndex = 0;
        for (; difficultyIndex < songInfo.metadata.difficulties.Length; difficultyIndex++)
        {
            if (songInfo.metadata.difficulties[difficultyIndex].name == difficulty.name) break;
        }

        songInfo.metadata.difficulties[difficultyIndex].rating = float.Parse(newRating);
        difficulty.rating = float.Parse(newRating);
    }

    public void OnDelete()
    {
        
        if (Directory.Exists(Path.Combine(Application.dataPath, difficulty.notePath)))
        {
            File.Delete(Path.Combine(Application.dataPath, difficulty.notePath));
        }
        
        List<Difficulty> tempList = songInfo.metadata.difficulties.ToList();
        tempList.RemoveAt(tempList.FindIndex(songDifficulty => {return songDifficulty.name == difficulty.name;}));
        songInfo.metadata.difficulties = tempList.ToArray();
    }

    public void OnEdit()
    {
        GameData.selectedDifficulty = difficulty;
        EditorSongsListSaver.instance.SaveSongs();
        SceneHandler.instance.LoadSceneAsync("SongEditorScene");
    }
}
