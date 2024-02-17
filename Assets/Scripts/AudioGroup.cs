using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Container class for multiple audio sources
public class AudioGroup : Dictionary<string, AudioSource>
{
    public void StopAll()
    {
        this.AsParallel().ForAll(audio => audio.Value.Stop());
    }

    public void PlayAll()
    {
        this.AsParallel().ForAll(audio => audio.Value.Play());
    }

    public void PauseAll()
    {
        this.AsParallel().ForAll(audio => audio.Value.Pause());
    }

    public void UnpauseAll()
    {
        this.AsParallel().ForAll(audio => audio.Value.UnPause());
    }

    public void ChangeVolumeAll(float newVolume)
    {
        this.AsParallel().ForAll(audio => audio.Value.volume = newVolume);
    }
}

