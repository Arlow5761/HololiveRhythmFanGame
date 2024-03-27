
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorDifficultyList : MonoBehaviour
{
    [SerializeField] private EditorDifficultyItemUI difficultyItemPrefab;
    [SerializeField] private Transform difficultyItemParent;

    private SongInfo songInfo;
    private List<EditorDifficultyItemUI> difficultyItems = new();

    public void OnSelectedSongChanged(SongInfo newSongInfo)
    {
        songInfo = newSongInfo;
        RefreshDifficultyItems();
    }

    public void CreateNewDifficulty()
    {
        List<Difficulty> tempList = songInfo.metadata.difficulties.ToList();
        tempList.Add(new() { name="New Difficulty", rating = 0f });
        songInfo.metadata.difficulties = tempList.ToArray();

        RefreshDifficultyItems();
    }

    public void RefreshDifficultyItems()
    {
        difficultyItems.ForEach(difficultyItem => { if (difficultyItem.gameObject != null) Destroy(difficultyItem.gameObject); });
        difficultyItems.Clear();

        for (int i = 0; i < songInfo.metadata.difficulties.Length; i++)
        {
            EditorDifficultyItemUI difficultyItem = Instantiate(difficultyItemPrefab, difficultyItemParent);
            difficultyItem.songInfo = songInfo;
            difficultyItem.difficulty = songInfo.metadata.difficulties[i];

            difficultyItems.Add(difficultyItem);
        }
    }
}
