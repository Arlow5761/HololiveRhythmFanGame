using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyDisplayer : MonoBehaviour
{
    public DifficultyBarDisplayer[] difficultyBars;

    public void UpdateDifficulties(Song song)
    {
        int i = 0;

        for (; i < song.metadata.difficulties.Length; i++)
        {
            difficultyBars[i].ChangeDifficulty(song.metadata.difficulties[i], 0, "S");
            difficultyBars[i].Display();
        }

        for (; i < difficultyBars.Length; i++)
        {
            difficultyBars[i].Hide();
        }
    }
}
