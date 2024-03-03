using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// Class that handles song selection in the song selection screen
public class SongSelector : MonoBehaviour
{
    public static SongSelector instance;

    private SongInfo _selectedSong;
    public SongInfo selectedSong
    {
        get
        {
            return _selectedSong;
        }

        set
        {
            _selectedSong = value;
            GameData.songInfo = _selectedSong;
            onSelectedSongChanged.Invoke(_selectedSong);
        }
    }


    public float rotationSpeed;
    public float rotationOffset;
    public int songSlots;
    public float radius;
    public float songSize;
    public GameObject songPrefab;
    public SongInfo[] songList;
    public UnityEvent<SongInfo> onSelectedSongChanged;
    

    private float rotation;
    private int rotateDirection;
    private int currentSong;
    private int currentSlot;
    private SongThumbnailDisplayer[] slotObjects = {};

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        songList = SongsListReader.instance.songList;

        Draw();
        //selectedSong = songList[currentSong];
        slotObjects[currentSlot].Focus();
        UpdateSlots();
    }

    void Update()
    {
        float currentRelativeRotation = GetRelativeRotation(currentSlot);
        
        if (currentRelativeRotation == 0f || currentRelativeRotation == 360f) return;

        if (currentRelativeRotation < 1 || currentRelativeRotation > 359)
        {
            rotation = 360f - (360f / songSlots * currentSlot);
        }
        else if (currentRelativeRotation > 180f)
        {
            rotation += rotationSpeed * Time.deltaTime;
        }
        else if (currentRelativeRotation < 180f)
        {
            rotation -= rotationSpeed * Time.deltaTime;
        }

        rotation %= 360f;

        if (rotation < 0)
        {
            rotation = 360f + rotation;
        }

        Draw(rotation);
    }

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void Draw(float rotation = 0)
    {
        if (slotObjects.Length != songSlots)
        {
            for (int i = 0; i < slotObjects.Length; i++)
            {
                Destroy(slotObjects[i]);
            }

            Array.Resize(ref slotObjects, songSlots);

            for (int i = 0; i < slotObjects.Length; i++)
            {
                slotObjects[i] = Instantiate(songPrefab, gameObject.transform).GetComponent<SongThumbnailDisplayer>();
                slotObjects[i].transform.localScale = new Vector3(songSize, songSize, 1);
            }
        }

        for (int i = 0; i < songSlots; i++)
        {
            float slotOffset = 360f / songSlots * i;

            float xPos = Mathf.Cos((rotation + rotationOffset + slotOffset) / 180f * Mathf.PI) * radius;
            float yPos = Mathf.Sin((rotation + rotationOffset + slotOffset) / 180f * Mathf.PI) * radius;

            slotObjects[i].transform.localPosition = new Vector3(xPos, yPos, 0);
        }
    }

    public void MoveCW()
    {
        float currentRelativeRotation = GetRelativeRotation(currentSlot);
        if (currentRelativeRotation != 0f && currentRelativeRotation != 360f) return;

        currentSong = Modulo(currentSong - 1, songList.Length);

        slotObjects[currentSlot].Unfocus();
        currentSlot = Modulo(currentSlot - 1, songSlots);
        slotObjects[currentSlot].Focus();

        selectedSong = songList[currentSong];

        UpdateSlots();

        AudioHandler.instance.GetSFX("confirmmusic").PlayOneShot();
    }

    public void MoveCCW()
    {
        float currentRelativeRotation = GetRelativeRotation(currentSlot);
        if (currentRelativeRotation != 0f && currentRelativeRotation != 360f) return;

        currentSong = Modulo(currentSong + 1, songList.Length);

        slotObjects[currentSlot].Unfocus();
        currentSlot = Modulo(currentSlot + 1, songSlots);
        slotObjects[currentSlot].Focus();

        selectedSong = songList[currentSong];

        UpdateSlots();

        AudioHandler.instance.GetSFX("confirmmusic").PlayOneShot();
    }

    public void UpdateSlots()
    {
        //for (int i = 0; i < slotObjects.Length; i++)
        //{
        //    slotObjects[i].displayedSong = songList[Modulo(i - currentSlot + currentSong, songList.Length)];
        //}
    }

    private float GetRelativeRotation(int slot)
    {
        float relativeRotation = Modulo(360f / songSlots * slot + rotation, 360f);

        return relativeRotation;
    }

    private static float Modulo(float a, float b)
    {
        float c = a % b;

        if (c < 0)
        {
            c += b;
        }

        return c;
    }

    private static int Modulo(int a, int b)
    {
        int c = a % b;

        if (c < 0)
        {
            c += b;
        }

        return c;
    }

    public void OnSceneLoaded()
    {
        Scores.ClearScores();
    }
}
