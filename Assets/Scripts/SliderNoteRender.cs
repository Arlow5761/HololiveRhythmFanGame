using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderNoteRender : NoteRender
{
    [SerializeField] GameObject trailPrefab;
    [SerializeField] GameObject endPrefab;

    GameObject trailObject;
    GameObject endObject;

    double timeEnd;

    void Start()
    {
        trailObject = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        endObject = Instantiate(endPrefab, transform.position, Quaternion.identity);

        timeInstantiated = noteData.TimestampStart - Song.Instance.noteTime;
        timeEnd = noteData.TimestampEnd - Song.Instance.noteTime;
        noteData.onHit.AddListener(OnHit);
    }

    void Update()
    {
        RenderNote();
    }

    void RenderNote()
    {
        RenderStart();
        RenderEnd();
        RenderTrail();
    }

    void RenderStart()
    {
        double timeSinceInstantiated = Song.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / Song.Instance.noteTime);

        // Debug.Log("Current t " + index + " : " + t);
        transform.localPosition = Vector3.LerpUnclamped(
            new Vector3(Song.Instance.noteSpawnX, transform.localPosition.y, 0), 
            new Vector3(Song.Instance.noteTapX, transform.localPosition.y, 0), 
            t
        );
        // transform.localPosition -= new Vector3(beatTempo * Time.deltaTime, 0, 0);
        // Debug.Log("x " + index + " : " + transform.localPosition.x);
        // Debug.Log("y " + index + " : " + transform.localPosition.y);
    }

    void RenderTrail()
    {
        float length = endObject.transform.position.x - transform.position.x;
        trailObject.transform.localScale = new Vector3(length, trailObject.transform.localScale.y, 1);

        float pos = (endObject.transform.position.x + transform.position.x) / 2;
        trailObject.transform.position = new Vector3(pos, transform.localPosition.y, 0);
    }

    void RenderEnd()
    {
        double timeSinceInstantiated = Song.GetAudioSourceTime() - timeEnd;
        float t = (float)(timeSinceInstantiated / Song.Instance.noteTime);

        // Debug.Log("Current t " + index + " : " + t);
        endObject.transform.localPosition = Vector3.LerpUnclamped(
            new Vector3(Song.Instance.noteSpawnX, transform.localPosition.y, 0), 
            new Vector3(Song.Instance.noteTapX, transform.localPosition.y, 0), 
            t
        );
        // transform.localPosition -= new Vector3(beatTempo * Time.deltaTime, 0, 0);
        // Debug.Log("x " + index + " : " + transform.localPosition.x);
        // Debug.Log("y " + index + " : " + transform.localPosition.y);
    }

    void OnHit(Grade grade)
    {

    }
}
