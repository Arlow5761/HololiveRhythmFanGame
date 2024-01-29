using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongThumbnailDisplayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SongData displayedSong
    {
        get
        {
            return _displayedSong;
        }

        set
        {
            _displayedSong = value;
            // Update thumbnail here
        }
    }
    public Vector3 unfocusedScale;
    public Vector3 focusedScale;
    public float focusSpeed;

    private bool isFocused;
    private SongData _displayedSong;

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
