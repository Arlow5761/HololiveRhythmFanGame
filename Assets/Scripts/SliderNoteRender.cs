using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

// Class to render hold notes
public class SliderNoteRender : NoteRender
{
    [SerializeField] GameObject trailPrefab;
    [SerializeField] GameObject endPrefab;

    GameObject trailObject;
    GameObject endObject;
    SpriteRenderer trailRenderer;

    double timeEnd;
    double holdTime = 0;
    bool isHit = false;
    bool missed = false;

    void Start()
    {
        trailObject = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        endObject = Instantiate(endPrefab, transform.position, Quaternion.identity);

        timeInstantiated = noteData.TimestampStart - Song.Instance.noteTime;
        timeEnd = noteData.TimestampEnd - Song.Instance.noteTime;
        noteData.onHit.AddListener(OnHit);

        trailRenderer = trailObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        RenderNote();
    }

    void OnDestroy()
    {
        Destroy(trailObject);
        Destroy(endObject);
    }

    void RenderNote()
    {
        RenderStart();
        RenderEnd();
        RenderTrail();
    }

    void RenderStart()
    {
        if (isHit && !missed)
        {
            holdTime = Song.GetAudioSourceTime() - noteData.TimestampStart;
            transform.localPosition = new Vector3(GameplayLayout.hitPosX, transform.localPosition.y, 0);
            return;
        }

        double timeSinceInstantiated = Song.GetAudioSourceTime() - timeInstantiated - holdTime;
        float t = (float)(timeSinceInstantiated / Song.Instance.noteTime);

        // Debug.Log("Current t " + index + " : " + t);
        transform.localPosition = Vector3.LerpUnclamped(
            new Vector3(GameplayLayout.noteSpawnX, transform.localPosition.y, 0), 
            new Vector3(GameplayLayout.hitPosX, transform.localPosition.y, 0), 
            t
        );
        // transform.localPosition -= new Vector3(beatTempo * Time.deltaTime, 0, 0);
        // Debug.Log("x " + index + " : " + transform.localPosition.x);
        // Debug.Log("y " + index + " : " + transform.localPosition.y);
    }

    void RenderTrail()
    {
        float length = endObject.transform.position.x - transform.position.x;
        trailRenderer.size = new Vector2(length, trailRenderer.size.y);

        float pos = (endObject.transform.position.x + transform.position.x) / 2;
        trailObject.transform.position = new Vector3(pos, transform.localPosition.y, 0);
    }

    void RenderEnd()
    {
        double timeSinceInstantiated = Song.GetAudioSourceTime() - timeEnd;
        float t = (float)(timeSinceInstantiated / Song.Instance.noteTime);

        // Debug.Log("Current t " + index + " : " + t);
        endObject.transform.localPosition = Vector3.LerpUnclamped(
            new Vector3(GameplayLayout.noteSpawnX, transform.localPosition.y, 0), 
            new Vector3(GameplayLayout.hitPosX, transform.localPosition.y, 0), 
            t
        );
        // transform.localPosition -= new Vector3(beatTempo * Time.deltaTime, 0, 0);
        // Debug.Log("x " + index + " : " + transform.localPosition.x);
        // Debug.Log("y " + index + " : " + transform.localPosition.y);

        if (endObject.transform.position.x < GameplayLayout.hitPosX)
        {
            missed = true;
        }

        if (endObject.transform.position.x < GameplayLayout.noteDespawnX)
        {
            CleanUp();
        }
    }

    void CleanUp()
    {
        noteData.onHit.RemoveListener(OnHit);
        Destroy(endObject);
        Destroy(trailObject);
        Destroy(gameObject);
    }

    void OnHit(Grade grade)
    {
        if (grade.name == "Miss")
        {
            missed = true;
        }
        else if (isHit)
        {
            CleanUp();
        }
        else
        {
            isHit = true;
        }
    }
}
