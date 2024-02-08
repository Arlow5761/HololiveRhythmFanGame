using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public struct Difficulty
{
    public string name;
    public float rating;
    public string notePath;
}

[Serializable]
public struct SongMetadata
{
    public string songName;
    public string artist;
    public string bpm;
    public Difficulty[] difficulties;
    public string songPath;
    public string coverPath;
}

[Serializable]
public class SongInfo
{
    public SongMetadata metadata;
}
