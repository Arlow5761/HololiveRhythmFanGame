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
    }
}

public class SongsDirectoryCleaner
{
    public static void CleanDirectory()
    {
        List<string> songFolders = Directory.GetDirectories(Path.Combine(Application.dataPath, "Songs")).ToList();

        for (int i = 0; i < SongsListReader.instance.songList.Count(); i++)
        {
            SongInfo songInfo = SongsListReader.instance.songList[i];

            songFolders.Remove(SongDirectoryFinder.FindSongDirectory(songInfo));
        }

        for (int i = 0; i < songFolders.Count(); i++)
        {
            Directory.Delete(songFolders[i]);
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

            folderContents.Remove(Path.Combine(Application.dataPath, songInfo.metadata.coverPath));
            folderContents.Remove(Path.Combine(Application.dataPath, songInfo.metadata.songPath));

            for (int i = 0; i < songInfo.metadata.difficulties.Count(); i++)
            {
                folderContents.Remove(Path.Combine(Application.dataPath, songInfo.metadata.difficulties[i].notePath));
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
        string expectedDirectory = Path.Combine(Application.dataPath, "Songs", songInfo.metadata.songName);

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
        string filePath = Path.Combine(Application.dataPath, songInfo.metadata.coverPath);

        if (File.Exists(filePath))
        {
            string targetFilePath = Path.Combine(SongDirectoryFinder.FindSongDirectory(songInfo), Path.GetFileName(filePath));

            if (targetFilePath != filePath)
            {
                File.Copy(filePath, targetFilePath);
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
        string filePath = Path.Combine(Application.dataPath, songInfo.metadata.songPath);

        if (File.Exists(filePath))
        {
            string targetFilePath = Path.Combine(SongDirectoryFinder.FindSongDirectory(songInfo), Path.GetFileName(filePath));

            if (targetFilePath != filePath)
            {
                File.Copy(filePath, targetFilePath);
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

            if (File.Exists(filePath) && targetFilePath != filePath)
            {
                File.Copy(filePath, targetFilePath);
            }
            else
            {
                StreamWriter streamWriter = File.CreateText(targetFilePath);

                // Setup difficulty defaults here

                streamWriter.Close();
            }

            songInfo.metadata.difficulties[i].notePath = Path.GetRelativePath(Application.dataPath, targetFilePath);
        }
    }
}
