using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EditorSongCoverField : MonoBehaviour
{
    [SerializeField] private Image coverImage;

    private SongInfo songInfo;

    void Start()
    {
        songInfo = EditorSongSelectionManager.instance.GetSelectedSong();
    }

    public void OnSelectionChanged(SongInfo newSongInfo)
    {
        songInfo = newSongInfo;
        RenderImageField();
    }

    public void ChangeSongCoverImage()
    {
        StartCoroutine(FilesPanel.OpenFilePanel("Choose New Cover Image", Application.dataPath, newCoverPath => {
            songInfo.metadata.coverPath = newCoverPath;
            EditorSongSelectionManager.instance.onSelectionModified.Invoke(songInfo);
            RenderImageField();
        }));
    }

    private void RenderImageField()
    {
        if (songInfo.metadata.coverPath == null) return;

        void onImageRetrieved(string receivedPath, Texture2D texture)
        {
            if (receivedPath != Path.Combine(Application.dataPath, songInfo.metadata.coverPath)) return;

            Sprite sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f), math.min(texture.width, texture.height), 0, SpriteMeshType.FullRect);
            coverImage.sprite = sprite;
            ImageLoader.onImageLoaded.RemoveListener(onImageRetrieved);
        }

        ImageLoader.onImageLoaded.AddListener(onImageRetrieved);

        StartCoroutine(ImageLoader.LoadImageFromFile(Path.Combine(Application.dataPath, songInfo.metadata.coverPath)));
    }
}
