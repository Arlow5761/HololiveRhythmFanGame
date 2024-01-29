using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public struct SongMetadata
{
    public string songName;
    public string artist;
    public string bpm;
    public string[] difficulties;
    public string notePath;
    public string songPath;
}

[Serializable]
public class SongInfo
{
    public SongMetadata metadata;
}
