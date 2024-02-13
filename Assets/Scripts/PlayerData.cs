using UnityEngine;
using UnityEngine.Events;

// Static class that stores serializable data (Mostly settings)
public static class PlayerData
{
    // Settings
    public static UnityEvent<float> onMusicVolumeChanged;
    public static UnityEvent<float> onSfxVolumeChanged;

    private static float _musicVolume = 1;
    public static float musicVolume
    {
        get
        {
            return _musicVolume;
        }

        set
        {
            _musicVolume = value;
            onMusicVolumeChanged.Invoke(_musicVolume);
        }
    }

    private static float _sfxVolume = 1;
    public static float sfxVolume
    {
        get
        {
            return _sfxVolume;
        }

        set
        {
            _sfxVolume = value;
            onSfxVolumeChanged.Invoke(_sfxVolume);
        }
    }

    public static double offset = 0;

    // Keybinds
    // later

    // Selection
    public static string character;
    public static string pet;

    public static void SaveData()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
        PlayerPrefs.SetString("Offset", offset.ToString());

        PlayerPrefs.Save();
    }

    public static void LoadData()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1);
        offset = double.Parse(PlayerPrefs.GetString("Offset"));
    }
}
