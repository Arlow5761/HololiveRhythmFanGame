using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

public static class JsonParser
{
    public static T OpenObject<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }

    public static T[] OpenArray<T>(string data)
    {
        string formatedData = "{\"array\":" + data + "}";

        return JsonUtility.FromJson<ArrayWrapper<T>>(formatedData).array;
    }

    private class ArrayWrapper<T>
    {
        public T[] array;
    }
}

public class SongsListReader : MonoBehaviour
{
    public static SongsListReader instance;

    public SongInfo[] songList;

    void Awake()
    {
        Initialize();
        ReadSongsListFile();
    }

    void Start()
    {
        
    }

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void ReadSongsListFile()
    {
        StreamReader stream = new StreamReader(Application.dataPath + @"\Songs\Songlist.json");
        string rawText = stream.ReadToEnd();
        SongMetadata[] metadataList = JsonParser.OpenArray<SongMetadata>(rawText);

        Array.Resize(ref songList, metadataList.Length);
        for (int i = 0; i < songList.Length; i++)
        {
            songList[i] = new SongInfo {metadata = metadataList[i]};
        }
    }
}
