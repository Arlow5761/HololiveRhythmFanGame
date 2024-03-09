using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class EditorTimeController : MonoBehaviour
{
    public static EditorTimeController instance;
    public float tickLength;
    public UnityEvent<double> onFirstTimestampChanged;
    public UnityEvent<int> onBeatDivisorChanged;

    private AudioSource audioSource;
    private double firstTimestamp = 0;
    private int beatDivisor = 4;
    private bool scrollForward = false;
    private bool scrollBackward = false;
    private float internalTimer = 0;

    void Awake()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    void Start()
    {
        audioSource = AudioSystem.instance.GetAudio("music", "music");
    }

    void Update()
    {
        if (scrollForward)
        {
            internalTimer += Time.deltaTime;

            if (internalTimer >= tickLength)
            {
                internalTimer = 0;
                StepForward();
            }
        }
        else if (scrollBackward)
        {
            internalTimer += Time.deltaTime;

            if (internalTimer >= tickLength)
            {
                internalTimer = 0;
                StepBack();
            }
        }
        else
        {
            internalTimer = 0;
        }
    }

    public void ScrollForward()
    {
        scrollForward = true;
        scrollBackward = false;
        StepForward();
    }

    public void ScrollBackward()
    {
        scrollForward = false;
        scrollBackward = true;
        StepBack();
    }

    public void StopScrollForward()
    {
        if (!scrollForward) return;

        scrollForward = false;
    }

    public void StopScrollBackward()
    {
        if (!scrollBackward) return;

        scrollBackward = false;
    }

    public void StepBack()
    {
        audioSource.timeSamples = (int) (GetPreviousStep(Song.GetAudioSourceTime()) * audioSource.clip.frequency);
    }

    public void StepForward()
    {
        audioSource.timeSamples = (int) (GetNextStep(Song.GetAudioSourceTime()) * audioSource.clip.frequency);
    }

    public void Goto(double timestamp)
    {
        audioSource.timeSamples = (int) (timestamp * audioSource.clip.frequency);
    }

    public void TogglePause()
    {
        if (audioSource.isPlaying)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void UnPause()
    {
        audioSource.UnPause();
    }

    public double GetNextStep(double current)
    {
        double bpm = int.Parse(GameData.songInfo.metadata.bpm);
        double normalizedTime = current - firstTimestamp;
        double divisor = 60d / beatDivisor;
        double timeInBeats = normalizedTime * bpm / divisor;
        double snapDelta = timeInBeats % 1;

        if (math.abs(snapDelta) <= 0.01 || math.abs(snapDelta) >= 0.99)
        {
            return (timeInBeats + 1) * divisor / bpm + firstTimestamp;
        }
        else
        {
            return math.ceil(timeInBeats) * divisor / bpm + firstTimestamp;
        }
    }

    public double GetPreviousStep(double current)
    {
        double bpm = int.Parse(GameData.songInfo.metadata.bpm);
        double normalizedTime = current - firstTimestamp;
        double divisor = 60d / beatDivisor;
        double timeInBeats = normalizedTime * bpm / divisor;
        double snapDelta = timeInBeats % 1;

        if (math.abs(snapDelta) <= 0.01 || math.abs(snapDelta) >= 0.99)
        {
            return (timeInBeats - 1) * divisor / bpm + firstTimestamp;
        }
        else
        {
            return math.floor(timeInBeats) * divisor / bpm + firstTimestamp;
        }
    }

    public void SetFirstTimestamp(double timestamp)
    {
        firstTimestamp = timestamp;
        onFirstTimestampChanged.Invoke(firstTimestamp);
    }

    public void SetFirstTimestampFromNotesData()
    {
        if (Song.Instance.NotesData.Any())
        {
            firstTimestamp = Song.Instance.NotesData.First().TimestampStart;
        }
        else
        {
            SetFirstTimestamp(0);
        }
    }

    public void SetBeatDivisor(int divisor)
    {
        beatDivisor = divisor;
        onBeatDivisorChanged.Invoke(beatDivisor);
    }
}
