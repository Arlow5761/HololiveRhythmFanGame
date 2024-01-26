using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SongSelector : MonoBehaviour
{
    public static SongSelector instance;

    public Song selectedSong;
    public float rotationSpeed;
    public float rotationOffset;
    public int songSlots;
    public float radius;
    public float songSize;
    public Song[] songList;
    public GameObject songPrefab;

    private float rotation;
    private int rotateDirection;
    private int currentSong;
    private int currentSlot;
    private GameObject[] slotObjects = {};

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        Draw();
    }

    void Update()
    {
        float currentRelativeRotation = (360f / songSlots * currentSlot + rotation) % 360f;

        if (currentRelativeRotation < 0)
        {
            currentRelativeRotation = 360f + currentRelativeRotation;
        }

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
                slotObjects[i] = Instantiate(songPrefab, gameObject.transform);
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
        currentSong = (currentSong - 1) % songList.Length;

        if (currentSong < 0)
        {
            currentSong = songList.Length - currentSong;
        }

        currentSlot = (currentSlot - 1) % songSlots;

        if (currentSlot < 0)
        {
            currentSlot = songSlots + currentSlot;
        }
    }

    public void MoveCCW()
    {
        currentSong = (currentSong + 1) % songList.Length;

        if (currentSong < 0)
        {
            currentSong = songList.Length - currentSong;
        }

        currentSlot = (currentSlot + 1) % songSlots;

        if (currentSlot < 0)
        {
            currentSlot = songSlots + currentSlot;
        }
    }
}
