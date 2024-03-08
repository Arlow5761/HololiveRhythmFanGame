using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DeletionHandler : MonoBehaviour
{
    public static DeletionHandler instance;

    [SerializeField] private Transform notesParent;

    private bool isAvailable;
    private bool isActive;

    public void Initialize()
    {
        if (instance != this && instance != null) return;

        instance = this;
    }

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        if (!isActive || !isAvailable) return;

        if (Input.GetMouseButtonDown(0))
        {
            OnPointerClick();
        }
    }

    public void Activate()
    {
        isActive = true;
        PlacementHandler.instance.HoldNothing();
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public void OnPointerAvailable()
    {
        isAvailable = true;
    }

    public void OnPointerUnavailable()
    {
        isAvailable = false;
    }

    public void OnPointerClick()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hitInfo.collider == null) return;

        GameObject noteObject = hitInfo.collider.gameObject;
        NoteRender noteRender = noteObject.GetComponent<NoteRender>();
        
        if (noteObject.transform.parent != notesParent || noteRender == null) return;
        
        Song.Instance.NotesData.Remove(noteRender.noteData);
        Destroy(noteObject);
    }
}
