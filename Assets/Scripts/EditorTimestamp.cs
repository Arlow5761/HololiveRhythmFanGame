using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditorTimestamp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timestampField;

    void Update()
    {
        double time = Song.GetAudioSourceTime();
        int timeInMilliseconds = (int) (time % 1 * 1000);
        int timeInSeconds = (int) (time % 60);
        int timeInMinutes = (int) time / 60;

        string timestamp = String.Format("{0:00}:{1:00}:{2:000}", timeInMinutes, timeInSeconds, timeInMilliseconds);
        timestampField.SetText(timestamp);
    }
}
