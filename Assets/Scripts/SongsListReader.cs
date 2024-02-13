using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

// Utility class to parse json using unity's jsonutility
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

    public static string WriteObject<T>(T data)
    {
        return JsonUtility.ToJson(data);
    }

    public static string WriteArray<T>(T[] data)
    {
        ArrayWrapper<T> wrappedObject = new() {array = data};
        string rawResult = JsonUtility.ToJson(wrappedObject);
        string result = rawResult.TrimStart(("{\"array\":").ToCharArray()).TrimEnd(("}").ToCharArray());
        return result;
    }

    [Serializable]
    private class ArrayWrapper<T>
    {
        public T[] array;
    }
}

// Singleton class to read the songlist file
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
