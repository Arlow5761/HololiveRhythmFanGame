using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EditorSongsListSaver : MonoBehaviour
{
    static public EditorSongsListSaver instance;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (instance != null && instance != this) return;
        instance = this;
    }

    public void SaveSongs()
    {
        for (int i = 0; i < SongsListReader.instance.songList.Count(); i++)
        {
            SongInfo songInfo = SongsListReader.instance.songList[i];

            SongDirectorySaver.SaveDirectory(songInfo);
            EditorSongCoverSaver.SaveCover(songInfo);
            EditorSongAudioSaver.SaveAudio(songInfo);
            EditorSongDifficultySaver.SaveDifficulty(songInfo);

            SongFolderCleaner.CleanFolder(songInfo);
        }

        SongsDirectoryCleaner.CleanDirectory();

        SongMetadata[] metadataArray = SongsListReader.instance.songList.ToList().ConvertAll((songInfo) => {return songInfo.metadata;}).ToArray();

        File.WriteAllText(
            Path.GetFullPath(Path.Join("Songs", "Songlist.json"), Application.dataPath),
            JsonParser.WriteArray(metadataArray));
    }
}

public class SongsDirectoryCleaner
{
    public static void CleanDirectory()
    {
        List<string> songFolders = Directory.GetDirectories(Path.Join(Path.GetFullPath(Application.dataPath), "Songs")).ToList();

        for (int i = 0; i < SongsListReader.instance.songList.Count(); i++)
        {
            SongInfo songInfo = SongsListReader.instance.songList[i];

            songFolders.Remove(SongDirectoryFinder.FindSongDirectory(songInfo));
        }

        for (int i = 0; i < songFolders.Count(); i++)
        {
            Directory.Delete(songFolders[i], true);
        }
    }
}

public class SongFolderCleaner
{
    public static void CleanFolder(SongInfo songInfo)
    {
        string folderPath = SongDirectoryFinder.FindSongDirectory(songInfo);

        if (folderPath != null)
        {
            List<string> folderContents = Directory.GetFiles(folderPath).ToList();
            folderContents.AddRange(Directory.GetDirectories(folderPath));

            folderContents.Remove(Path.GetFullPath(songInfo.metadata.coverPath, Application.dataPath));
            folderContents.Remove(Path.GetFullPath(songInfo.metadata.songPath, Application.dataPath));

            for (int i = 0; i < songInfo.metadata.difficulties.Count(); i++)
            {
                folderContents.Remove(Path.GetFullPath(songInfo.metadata.difficulties[i].notePath, Application.dataPath));
            }

            for (int i = 0; i < folderContents.Count(); i++)
            {
                if (Directory.Exists(folderContents[i]))
                {
                    Directory.Delete(folderContents[i]);
                }
                else if (File.Exists(folderContents[i]))
                {
                    File.Delete(folderContents[i]);
                }
            }
        }
    }
}

public class SongDirectoryFinder
{
    public static string FindSongDirectory(SongInfo songInfo)
    {
        string expectedDirectory = Path.Join(Path.GetFullPath(Application.dataPath), "Songs", songInfo.metadata.songName);

        if (Directory.Exists(expectedDirectory))
        {
            return expectedDirectory;
        }
        else
        {
            return null;
        }
    }
}

public class SongDirectorySaver
{
    public static void SaveDirectory(SongInfo songInfo)
    {
        if (SongDirectoryFinder.FindSongDirectory(songInfo) == null)
        {
            string newDirectory = Path.Combine(Application.dataPath, "Songs", songInfo.metadata.songName);

            Directory.CreateDirectory(newDirectory);
        }
    }
}

public class EditorSongCoverSaver
{
    public static void SaveCover(SongInfo songInfo)
    {
        string filePath;

        if (Path.IsPathFullyQualified(songInfo.metadata.coverPath))
        {
            filePath = songInfo.metadata.coverPath;
        }
        else
        {
            filePath = Path.GetFullPath(songInfo.metadata.coverPath, Application.dataPath);
        }

        if (File.Exists(filePath))
        {
            string targetFilePath = Path.Combine(SongDirectoryFinder.FindSongDirectory(songInfo), Path.GetFileName(filePath));

            if (Path.GetFullPath(targetFilePath) != Path.GetFullPath(filePath))
            {
                File.Copy(filePath, targetFilePath, true);
                songInfo.metadata.coverPath = Path.GetRelativePath(Application.dataPath, targetFilePath);
            }
        }
        else
        {
            Debug.LogError("Cover File Not Found!");
        }
    }
}

public class EditorSongAudioSaver
{
    public static void SaveAudio(SongInfo songInfo)
    {
        string filePath;

        if (Path.IsPathFullyQualified(songInfo.metadata.songPath))
        {
            filePath = songInfo.metadata.songPath;
        }
        else
        {
            filePath = Path.GetFullPath(songInfo.metadata.songPath, Application.dataPath);
        }

        if (File.Exists(filePath))
        {
            string targetFilePath = Path.Combine(SongDirectoryFinder.FindSongDirectory(songInfo), Path.GetFileName(filePath));

            if (Path.GetFullPath(targetFilePath) != Path.GetFullPath(filePath))
            {
                File.Copy(filePath, targetFilePath, true);
                songInfo.metadata.songPath = Path.GetRelativePath(Application.dataPath, targetFilePath);
            }
        }
        else
        {
            Debug.LogError("Audio File Not Found!");
        }
    }
}

public class EditorSongDifficultySaver
{
    public static void SaveDifficulty(SongInfo songInfo)
    {
        for (int i = 0; i < songInfo.metadata.difficulties.Count(); i++)
        {
            string filePath = Path.Combine(Application.dataPath, songInfo.metadata.difficulties[i].notePath);
            string targetFilePath = Path.Combine(SongDirectoryFinder.FindSongDirectory(songInfo), Path.GetFileName(filePath));

            if (File.Exists(filePath) && (Path.GetFullPath(targetFilePath) != Path.GetFullPath(filePath)))
            {
                File.Copy(filePath, targetFilePath, true);
            }
            else
            {
                StreamWriter streamWriter = File.CreateText(targetFilePath);

                LevelData newLevelData = new()
                {
                    TickSpeed = 0.3680981595d,
                    NoteSpeed = 1d,
                    BaseDamage = 20,
                    BaseHeal = 15,
                    BaseFeverIncrease = 10,
                };
                newLevelData.NormalThresholds = new Grade[3];
                newLevelData.NormalThresholds[0] = new() {name = "Perfect", margin = 0.05, score = 40};
                newLevelData.NormalThresholds[1] = new() {name = "Great", margin = 0.1, score = 20};
                newLevelData.NormalThresholds[2] = new() {name = "Okay", margin = 0.2, score = 10};
                newLevelData.SpecialThresholds = new Grade[4];
                newLevelData.SpecialThresholds[0] = new() {name = "Miss", margin = 0, score = 0};
                newLevelData.SpecialThresholds[1] = new() {name = "SliderTick", margin = 0, score = 1};
                newLevelData.SpecialThresholds[2] = new() {name = "MashGrade", margin = 0, score = 10};
                newLevelData.SpecialThresholds[3] = new() {name = "ScoreNote", margin = 0, score = 20};

                streamWriter.Write(JsonParser.WriteObject(newLevelData));
                streamWriter.Close();
            }

            songInfo.metadata.difficulties[i].notePath = Path.GetRelativePath(Application.dataPath, targetFilePath);
        }
    }
}
