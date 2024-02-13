using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class ScoreContainer
{
    public string grade;
    public int score;
    public int combo;
    public float accuracy;
    public int perfect;
    public int great;
    public int okay;
    public int miss;
}

[Serializable]
public class DifficultyScoresContainer
{
    public string name;
    public ScoreContainer[] scores;
}

[Serializable]
public class SongScoresContainer
{
    public string songName;
    public DifficultyScoresContainer[] difficulties;
}

[Serializable]
public class SongScoresPath
{
    public string songName;
    public string path;
}

// Static class that handles score calculations, saving, and loading scores
public static class Scores
{
    public static int score = 0;
    public static int combo = 0;
    public static bool fullCombo = true;
    public static Dictionary<string, int> grades = new() { {"Perfect", 0}, {"Great", 0}, {"Okay", 0}, {"Miss", 0} };
    public static float accuracy = 0;
    public static string finalGrade;

    public static void ClearScores()
    {
        score = 0;
        combo = 0;
        fullCombo = true;
        grades["Perfect"] = 0;
        grades["Great"] = 0;
        grades["Okay"] = 0;
        grades["Miss"] = 0;
        accuracy = 0;
        finalGrade = null;
    }

    public static void SaveScores()
    {
        SongScoresContainer songScoresContainer = LoadScores(GameData.songInfo.metadata.songName);

        if (songScoresContainer == null)
        {
            SongScoresPath[] songScoresPaths = JsonParser.OpenArray<SongScoresPath>(File.ReadAllText(Path.Combine(Application.dataPath, "Scores", "ScorePaths.json")));

            string newPath = Path.Combine("Scores", GameData.songInfo.metadata.songName.Replace(" ", "") + ".json");
            FileStream newFile = File.Create(Path.Combine(Application.dataPath, newPath));
            newFile.Close();

            SongScoresContainer newContainer = new() {songName = GameData.songInfo.metadata.songName, difficulties = new DifficultyScoresContainer[0]};
            string newScoreData = JsonParser.WriteObject(newContainer);
            File.WriteAllText(Path.Combine(Application.dataPath, newPath), newScoreData);

            songScoresPaths = songScoresPaths.Append(new SongScoresPath() {songName = GameData.songInfo.metadata.songName, path = newPath}).ToArray();
            
            string jsonData = JsonParser.WriteArray(songScoresPaths);
            File.WriteAllText(Path.Combine(Application.dataPath, "Scores", "ScorePaths.json"), jsonData);

            songScoresContainer = JsonParser.OpenObject<SongScoresContainer>(File.ReadAllText(Path.Combine(Application.dataPath, newPath)));
        }

        DifficultyScoresContainer difficultyScoreContainer = songScoresContainer.difficulties.FirstOrDefault(x => { return x.name == GameData.selectedDifficulty.name; });

        if (difficultyScoreContainer == null)
        {
            difficultyScoreContainer = new DifficultyScoresContainer() {name = GameData.selectedDifficulty.name, scores = new ScoreContainer[0]};
            songScoresContainer.difficulties = songScoresContainer.difficulties.Append(difficultyScoreContainer).ToArray();
        }

        difficultyScoreContainer.scores = difficultyScoreContainer.scores.Append(new()
        {
            grade = finalGrade,
            score = score,
            combo = combo,
            accuracy = accuracy,
            perfect = grades["Perfect"],
            great = grades["Great"],
            okay = grades["Okay"],
            miss = grades["Miss"]
        }).ToArray();

        string savePath = Path.Combine("Scores", GameData.songInfo.metadata.songName.Replace(" ", "") + ".json");
        string savedScoreData = JsonParser.WriteObject(songScoresContainer);
        File.WriteAllText(Path.Combine(Application.dataPath, savePath), savedScoreData);
    }

    public static SongScoresContainer LoadScores(string songName)
    {
        SongScoresPath[] songScoresPaths = JsonParser.OpenArray<SongScoresPath>(File.ReadAllText(Path.Combine(Application.dataPath, "Scores", "ScorePaths.json")));

        SongScoresPath songScoresPath = songScoresPaths.FirstOrDefault(x => { return x.songName == songName; });

        if (songScoresPath == null) return null;

        SongScoresContainer allDifficultyScores = JsonParser.OpenObject<SongScoresContainer>(File.ReadAllText(Path.Combine(Application.dataPath, songScoresPath.path)));

        return allDifficultyScores;
    }

    public static string GetFinalGrade()
    {
        if (accuracy >= 100)
        {
            finalGrade = "SS";
        }
        else if (accuracy >= 90 && fullCombo)
        {
            finalGrade = "S";
        }
        else if (accuracy >= 85)
        {
            finalGrade = "A";
        }
        else if (accuracy >= 75)
        {
            finalGrade = "B";
        }
        else if (accuracy >= 50)
        {
            finalGrade = "C";
        }
        else
        {
            finalGrade = "F";
        }

        return finalGrade;
    }

    public static float CalculateAccuracy()
    {
        int noteCount = grades.Values.Sum();

        return accuracy = (grades["Perfect"] + grades["Great"] * 0.5f + grades["Okay"] * 0.25f ) / noteCount * 100;
    }
}