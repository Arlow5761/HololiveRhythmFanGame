using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealNoteRender : NoteRender
{
    void Start()
    {
        timeInstantiated = noteData.TimestampStart - Song.Instance.noteTime;
        noteData.onHit.AddListener(OnHit);
    }

    void Update()
    {
        RenderNote();
    }

    void RenderNote()
    {
        double timeSinceInstantiated = Song.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / Song.Instance.noteTime);

        // Debug.Log("Current t " + index + " : " + t);
        transform.localPosition = Vector3.LerpUnclamped(
            new Vector3(GameplayLayout.noteSpawnX - GameplayLayout.hitPosX + GameplayLayout.playerPosX, transform.localPosition.y, 0), 
            new Vector3(GameplayLayout.playerPosX, transform.localPosition.y, 0), 
            t
        );
        // transform.localPosition -= new Vector3(beatTempo * Time.deltaTime, 0, 0);
        // Debug.Log("x " + index + " : " + transform.localPosition.x);
        // Debug.Log("y " + index + " : " + transform.localPosition.y);

        if (transform.localPosition.x <= GameplayLayout.noteDespawnX)
        {
            CleanUp();
        }
    }

    void OnHit(Grade grade)
    {
        CleanUp();
    }

    void CleanUp()
    {
        noteData.onHit.RemoveListener(OnHit);
        Destroy(gameObject);
    }
}
