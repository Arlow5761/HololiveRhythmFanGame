using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore;

public class PlacementHandler : MonoBehaviour
{
    static public PlacementHandler instance;

    private string heldObjectName = null;
    private NoteRender heldObject = null;
    private NoteData temporaryNoteData = null;
    private bool editingTail = false;

    void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        if (temporaryNoteData == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            OnPointerClick();
        }

        if (editingTail && temporaryNoteData.TimestampEnd != GetClosestTime())
        {
            temporaryNoteData.TimestampEnd = GetClosestTime();
            
            Destroy(heldObject.gameObject);
            heldObject = GenerateNote(temporaryNoteData);
        }
        else if (!editingTail && (temporaryNoteData.RowNumber != GetClosestLane() || temporaryNoteData.TimestampStart != GetClosestTime()))
        {
            temporaryNoteData.TimestampStart = GetClosestTime();
            temporaryNoteData.TimestampEnd = GetClosestTime();
            temporaryNoteData.RowNumber = GetClosestLane();
            Destroy(heldObject.gameObject);

            heldObject = GenerateNote(temporaryNoteData);
        }
    }

    public void SwitchHeldObject(string newHeldObjectName)
    {
        heldObjectName = newHeldObjectName;
        DeletionHandler.instance.Deactivate();
    }

    public void HoldNothing()
    {
        heldObjectName = null;
        if (heldObject != null) Destroy(heldObject);
    }

    public void OnPointerAvailable()
    {
        if (heldObject != null) Destroy(heldObject.gameObject);
        if (heldObjectName == null) return;

        temporaryNoteData = new()
        {
            RowNumber = GetClosestLane(),
            TimestampStart = GetClosestTime(),
            TimestampEnd = GetClosestTime(),
            NoteType = heldObjectName,
            NoteId = heldObjectName switch
            {
                "Normal" => 1,
                "Hold" => 2,
                "Mash" => 3,
                "Heal" => 4,
                "Obstacle" => 5,
                "Score" => 6,
                _ => 0
            }
        };

        heldObject = GenerateNote(temporaryNoteData);
    }

    public void OnPointerUnavailable()
    {
        if (heldObject != null) Destroy(heldObject.gameObject);
        temporaryNoteData = null;
        editingTail = false;
    }

    public void OnPointerClick()
    {
        switch (temporaryNoteData.NoteType)
        {
            case "Hold":
                if (!editingTail)
                {
                    editingTail = true;

                    Destroy(heldObject.gameObject);
                    heldObject = GenerateNote(temporaryNoteData);
                }
                else
                {
                    int id = Song.Instance.NotesData.FindIndex((noteData) => { return noteData.TimestampStart > temporaryNoteData.TimestampStart; });
                    if (id == -1) id = Song.Instance.NotesData.Count;

                    Song.Instance.NotesData.Insert(id, temporaryNoteData);
                    temporaryNoteData = new()
                    {
                        TimestampStart = temporaryNoteData.TimestampStart,
                        TimestampEnd = temporaryNoteData.TimestampEnd,
                        NoteId = temporaryNoteData.NoteId,
                        RowNumber = temporaryNoteData.RowNumber,
                        NoteType = temporaryNoteData.NoteType
                    };

                    Destroy(heldObject.gameObject);
                    heldObject = GenerateNote(temporaryNoteData);
                }
            break;
            default:
                int index = Song.Instance.NotesData.FindIndex((noteData) => { return noteData.TimestampStart > temporaryNoteData.TimestampStart; });
                if (index == -1) index = Song.Instance.NotesData.Count;

                Song.Instance.NotesData.Insert(index, temporaryNoteData);
                temporaryNoteData = new()
                {
                    TimestampStart = temporaryNoteData.TimestampStart,
                    TimestampEnd = temporaryNoteData.TimestampEnd,
                    NoteId = temporaryNoteData.NoteId,
                    RowNumber = temporaryNoteData.RowNumber,
                    NoteType = temporaryNoteData.NoteType
                };

                Destroy(heldObject.gameObject);
                heldObject = GenerateNote(temporaryNoteData);
            break;
        }
    }

    private NoteRender GenerateNote(NoteData data)
    {
        Vector3 position = data.RowNumber switch
        {
            0 => new Vector3(GameplayLayout.noteSpawnX, GameplayLayout.groundLaneY, 0),
            1 => new Vector3(GameplayLayout.noteSpawnX, GameplayLayout.airLaneY, 0),
            _ => new Vector3(GameplayLayout.noteSpawnX, 0, 0),
        };

        GameObject noteObject = Instantiate(Resources.Load<GameObject>("Note" + data.NoteId), position, new Quaternion(0, 0, 0, 1));
        noteObject.GetComponent<NoteRender>().noteData = data;

        return noteObject.GetComponent<NoteRender>();
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private int GetClosestLane()
    {
        float middleLine = (GameplayLayout.airLaneY + GameplayLayout.groundLaneY) / 2;

        if (GetMousePosition().y > middleLine)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private double GetClosestTime()
    {
        double relativeDistance = (double) (GetMousePosition().x - GameplayLayout.hitPosX) / (GameplayLayout.noteSpawnX - GameplayLayout.hitPosX);
        double relativeTime = relativeDistance * Song.Instance.noteTime;
        double timestamp = Song.GetAudioSourceTime() + relativeTime;

        double nextStep = EditorTimeController.instance.GetNextStep(timestamp);
        double lastStep = EditorTimeController.instance.GetPreviousStep(timestamp);

        if (nextStep - timestamp > timestamp - lastStep)
        {
            return lastStep;
        }
        else
        {
            return nextStep;
        }
    }
}
