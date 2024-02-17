using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

// Class that handles audio loading from files
public static class AudioLoader
{
    private static Dictionary<string, AudioClip> audioCache = new();
    public static UnityEvent<string, AudioClip> onAudioLoaded = new();
    public static UnityEvent<string> onAudioLoadFailed = new();

    public static IEnumerator LoadAudioFromFile(string path, AudioType audioType = AudioType.UNKNOWN)
    {
        // Get file type if not specified
        if (audioType == AudioType.UNKNOWN)
        {
            audioType = DeduceAudioType(path);
        }

        AudioClip audioClip;

        // Check if audio is already cached
        if (audioCache.TryGetValue(path, out audioClip))
        {
            onAudioLoaded.Invoke(path, audioClip);
        }
        else
        {
            // Load the audio from file
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, audioType))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    // Set it to stream the data instead of loading it in one frame
                    ((DownloadHandlerAudioClip) www.downloadHandler).streamAudio = true;
                    audioClip = ((DownloadHandlerAudioClip) www.downloadHandler).audioClip;

                    // Check if audio clip is valid
                    if (audioClip == null)
                    {
                        Debug.LogError("Audio clip is null");
                        onAudioLoadFailed.Invoke(path);
                    }
                    else
                    {
                        // Cache the audio clip
                        if (audioCache.TryAdd(path, audioClip))
                        {
                            onAudioLoaded.Invoke(path, audioClip);
                        }
                        else
                        {
                            // For good measure
                            onAudioLoaded.Invoke(path, audioCache[path]);
                        }
                    }
                }
                else
                {
                    Debug.LogError("Error loading audio: \"" + path + "\"");
                }
            }
        }
    }

    // Returns the probable audio type based on the file's extension
    private static AudioType DeduceAudioType(string path)
    {
        string extension = path.Split(".").Last();

        AudioType audioType = extension switch
        {
            "ogg" => AudioType.OGGVORBIS,
            "mp2" => AudioType.MPEG,
            "mp3" => AudioType.MPEG,
            "mp4" => AudioType.MPEG,
            "wav" => AudioType.WAV,
            "xma" => AudioType.XMA,
            "xm" => AudioType.XM,
            "vag" => AudioType.VAG,
            "s3m" => AudioType.S3M,
            "aiff" => AudioType.AIFF,
            "aif" => AudioType.AIFF,
            _ => AudioType.UNKNOWN
        };

        return audioType;
    }
}
