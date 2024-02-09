using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyDisplayer : MonoBehaviour
{
    [SerializeField] private DifficultyBarDisplayer[] difficultyBars;
    [SerializeField] private SpriteContainer gradeSprites;

    public void UpdateDifficulties(SongInfo song)
    {
        SongScoresContainer songScores = Scores.LoadScores(song.metadata.songName);

        int i = 0;

        for (; i < song.metadata.difficulties.Length; i++)
        {
            Sprite gradeSprite = null;

            if (songScores != null)
            {
                DifficultyScoresContainer difficultyScores = songScores.difficulties.FirstOrDefault(x => {return x.name == song.metadata.difficulties[i].name;});

                if (difficultyScores != null)
                {
                    ScoreContainer maxScore = difficultyScores.scores.First();

                    for (int j = 1; j < difficultyScores.scores.Count(); j++)
                    {
                        if (difficultyScores.scores[j].score > maxScore.score)
                        {
                            maxScore = difficultyScores.scores[j];
                        }
                    }

                    gradeSprite = gradeSprites.GetSprite(maxScore.grade);
                }
            }
            
            difficultyBars[i].ChangeDifficulty(song.metadata.difficulties[i], gradeSprite);
            difficultyBars[i].Display();
        }

        for (; i < difficultyBars.Length; i++)
        {
            difficultyBars[i].Hide();
        }
    }
}
