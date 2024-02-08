using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Vector2 anchor;
    public float scale;

    [SerializeField] private SpriteRenderer spriteRenderer;

    void Update()
    {
        Vector2 mouseOffset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mouseDirection = mouseOffset * 2 / math.sqrt(Screen.width * Screen.width + Screen.height * Screen.height);

        transform.position = -mouseDirection * scale;
    }

    public void ChangeBackground(string path)
    {
        void OnBackgroundLoaded(string receivedPath, Texture2D texture)
        {
            if (receivedPath != Path.Combine(Application.dataPath, path)) return;

            Sprite sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f), texture.width, 0, SpriteMeshType.FullRect);
            spriteRenderer.sprite = sprite;
            ImageLoader.onImageLoaded.RemoveListener(OnBackgroundLoaded);
        }

        ImageLoader.onImageLoaded.AddListener(OnBackgroundLoaded);

        StartCoroutine(ImageLoader.LoadImageFromFile(Path.Combine(Application.dataPath, path)));
    }

    public void OnSongChanged(SongInfo songInfo)
    {
        ChangeBackground(songInfo.metadata.coverPath);
    }

    public void ChangeToCurrentGameData()
    {
        ChangeBackground(GameData.songInfo.metadata.coverPath);
    }
}
