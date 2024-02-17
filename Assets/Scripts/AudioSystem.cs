using System.Collections.Generic;
using UnityEngine;

// Singleton class to manage game audio
public class AudioSystem : MonoBehaviour
{
    public static AudioSystem instance;

    private Dictionary<string, AudioGroup> audioGroups = new();

    // Finds and returns the AudioSource responsible for playing a ceratin sound
    // If the AudioSource is not found, returns null
    public AudioSource GetAudio(string groupName, string audioName)
    {
        AudioGroup tempAudioGroup;
        AudioSource tempAudioSource;

        if (!audioGroups.TryGetValue(groupName, out tempAudioGroup)) return null;
        if (!tempAudioGroup.TryGetValue(audioName, out tempAudioSource)) return null;

        return tempAudioSource;
    }

    // Finds and returns the AudioSource responsible for playing a ceratin sound
    // If the AudioSource is not found, returns null
    public AudioSource GetAudio(string audioName)
    {
        foreach ((string groupName, AudioGroup audioGroup) in audioGroups)
        {
            AudioSource tempAudioSource;

            if (audioGroup.TryGetValue(audioName, out tempAudioSource))
            {
                return tempAudioSource;
            }
        }

        return null;
    }

    // Returns the specified audio group
    // Returns null if it doesn't exist
    public AudioGroup GetAudioGroup(string groupName)
    {
        return audioGroups.GetValueOrDefault(groupName, null);
    }

    // Creates new audio along with the gameobject containing an audiosource to control it
    // Returns null if the audio already exists
    public AudioSource CreateNewAudio(string groupName, string audioName, AudioClip audioClip = null)
    {
        AudioGroup audioGroup;

        if (!audioGroups.TryGetValue(groupName, out audioGroup))
        {
            GameObject groupObject = new GameObject(groupName);
            groupObject.transform.SetParent(transform);

            GameObject audioObject = new GameObject(audioName);
            audioObject.transform.SetParent(groupObject.transform);

            AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;

            audioGroups.Add(groupName, new() {{audioName, newAudioSource}});
            return newAudioSource;
        }

        if (!audioGroup.ContainsKey(audioName))
        {
            GameObject groupObject = transform.Find(groupName).gameObject;

            GameObject audioObject = new GameObject(audioName);
            audioObject.transform.SetParent(groupObject.transform);

            AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;

            audioGroup.Add(audioName, newAudioSource);
            return newAudioSource;
        }

        return null;
    }

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;

        RegisterStartingSetup();
    }

    // Reads the hierarchy of the game object and create audio groups accordingly
    private void RegisterStartingSetup()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject groupObject = transform.GetChild(i).gameObject;
            AudioGroup audioGroup = new();
            audioGroups.TryAdd(groupObject.name, audioGroup);

            for (int j = 0; j < groupObject.transform.childCount; j++)
            {
                GameObject audioObject = groupObject.transform.GetChild(j).gameObject;
                AudioSource audioSource = audioObject.GetComponent<AudioSource>();
                audioGroup.TryAdd(audioObject.name, audioSource);   
            }
        }
    }

    void Awake()
    {
        Initialize();
    }
}

