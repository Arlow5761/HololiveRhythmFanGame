using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

// Class that handles image loading
public static class ImageLoader
{
    private static Dictionary<string, Texture2D> imageCache = new();
    public static UnityEvent<string, Texture2D> onImageLoaded = new();
    public static UnityEvent<string> onImageLoadFailed = new();

    public static IEnumerator LoadImageFromFile(string path)
    {
        Texture2D image;
        if (imageCache.TryGetValue(path, out image))
        {
            onImageLoaded.Invoke(path, image);
        }
        else
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(path))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    image =  DownloadHandlerTexture.GetContent(www);

                    if (image == null)
                    {
                        Debug.LogError("Image is null");
                        onImageLoadFailed.Invoke(path);
                    }
                    else
                    {
                        if (imageCache.TryAdd(path, image))
                        {
                            onImageLoaded.Invoke(path, image);
                        }
                        else
                        {
                            onImageLoaded.Invoke(path, imageCache[path]);
                        }
                    }
                }
                else
                {
                    Debug.LogError("Error loading image: \"" + path + "\"");
                }
            }
        }
    }
}
