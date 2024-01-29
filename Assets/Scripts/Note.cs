using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = Song.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = Song.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (Song.Instance.noteTime * 2));

        if (t <= 1)
        {
            Debug.Log("Current t " + index + " : " + t);
            transform.localPosition = Vector3.Lerp(
                new Vector3(Song.Instance.noteSpawnX, transform.localPosition.y, 0), 
                new Vector3(Song.Instance.noteDespawnX, transform.localPosition.y, 0), 
                t
            );
            // transform.localPosition -= new Vector3(beatTempo * Time.deltaTime, 0, 0);
            Debug.Log("x " + index + " : " + transform.localPosition.x);
            Debug.Log("y " + index + " : " + transform.localPosition.y);
        }
    }
}
