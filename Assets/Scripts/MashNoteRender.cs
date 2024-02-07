using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MashNoteRender : NoteRender
{
    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = noteData.TimestampStart - Song.Instance.noteTime;
        noteData.onHit.AddListener(OnHit);
    }

    // Update is called once per frame
    void Update()
    {
        RenderNote();
    }

    void RenderNote()
    {
        double timeSinceInstantiated = Song.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / Song.Instance.noteTime);

        // Debug.Log("Current t " + index + " : " + t);
        transform.localPosition = Vector3.Lerp(
            new Vector3(Song.Instance.noteSpawnX, transform.localPosition.y, 0), 
            new Vector3(Song.Instance.noteTapX, transform.localPosition.y, 0), 
            t
        );
        // transform.localPosition -= new Vector3(beatTempo * Time.deltaTime, 0, 0);
        // Debug.Log("x " + index + " : " + transform.localPosition.x);
        // Debug.Log("y " + index + " : " + transform.localPosition.y);

        if (Song.GetAudioSourceTime() > noteData.TimestampEnd)
        {
            CleanUp();
        }
    }

    void OnHit(Grade grade)
    {
        PlayerController.instance.whiff = false;
    }

    void CleanUp()
    {
        noteData.onHit.RemoveListener(OnHit);
        Destroy(gameObject);
    }
}
