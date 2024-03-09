using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EditorTimelineSlider : MonoBehaviour
{
    [SerializeField] private Slider timeline;

    private AudioSource musicSource;

    void Start()
    {
        musicSource = AudioSystem.instance.GetAudio("music", "music");
    }

    void Update()
    {
        int musicSamplePosition = musicSource.timeSamples;
        float musicNormalizedPosition = (float) musicSamplePosition / musicSource.clip.samples;

        timeline.SetValueWithoutNotify(musicNormalizedPosition);
    }

    public void OnNormalizedTimeChanged(float normalizedTime)
    {
        int musicSamplePosition = math.min((int) (normalizedTime * musicSource.clip.samples), musicSource.clip.samples - 1);

        musicSource.timeSamples = musicSamplePosition;
    }
}
