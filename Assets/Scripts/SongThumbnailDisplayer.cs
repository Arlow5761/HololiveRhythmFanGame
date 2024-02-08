using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class SongThumbnailDisplayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SongInfo displayedSong
    {
        get
        {
            return _displayedSong;
        }

        set
        {
            _displayedSong = value;
            
            void OnBackgroundLoaded(string receivedPath, Texture2D texture)
            {
                if (receivedPath != Path.Combine(Application.dataPath, displayedSong.metadata.coverPath)) return;

                Sprite sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f), math.min(texture.width, texture.height), 0, SpriteMeshType.FullRect);
                spriteRenderer.sprite = sprite;
                ImageLoader.onImageLoaded.RemoveListener(OnBackgroundLoaded);
            }

            ImageLoader.onImageLoaded.AddListener(OnBackgroundLoaded);

            StartCoroutine(ImageLoader.LoadImageFromFile(Path.Combine(Application.dataPath, displayedSong.metadata.coverPath)));
        }
    }
    public Vector3 unfocusedScale;
    public Vector3 focusedScale;
    public float focusSpeed;

    private bool isFocused;
    private SongInfo _displayedSong;

    void Update()
    {
        if (isFocused && gameObject.transform.localScale != focusedScale)
        {
            float scaleX = Mathf.Min(gameObject.transform.localScale.x + focusSpeed * Time.deltaTime, focusedScale.x);
            float scaleY = Mathf.Min(gameObject.transform.localScale.y + focusSpeed * Time.deltaTime, focusedScale.y);
            float scaleZ = Mathf.Min(gameObject.transform.localScale.z + focusSpeed * Time.deltaTime, focusedScale.z);

            gameObject.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
        else if (!isFocused && gameObject.transform.localScale != unfocusedScale)
        {
            float scaleX = Mathf.Max(gameObject.transform.localScale.x - focusSpeed * Time.deltaTime, unfocusedScale.x);
            float scaleY = Mathf.Max(gameObject.transform.localScale.y - focusSpeed * Time.deltaTime, unfocusedScale.y);
            float scaleZ = Mathf.Max(gameObject.transform.localScale.z - focusSpeed * Time.deltaTime, unfocusedScale.z);

            gameObject.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
    }

    public void Focus()
    {
        isFocused = true;
    }

    public void Unfocus()
    {
        isFocused = false;
    }
}
