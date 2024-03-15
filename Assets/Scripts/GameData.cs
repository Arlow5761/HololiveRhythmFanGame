// Static class to store general game data
using UnityEditor;

public static class GameData
{
    public static SongInfo songInfo = new() // Default value for testing
    {
        metadata = new()
        {
            songName = "Mantra Hujan",
            artist = "Kobo Kanaeru",
            bpm = "160",
            difficulties = new Difficulty[]
            {
                new()
                {
                    name = "Banjir",
                    rating = 7,
                    notePath = "Songs//MantraHujan//Banjir.json"
                }
            },
            songPath = "Songs//MantraHujan//Mantra-Hujan-Kobo-Kanaeru.ogg",
            coverPath = "Songs//MantraHujan//background.jpg"
        }
    };
    public static Difficulty selectedDifficulty = songInfo.metadata.difficulties[0]; // Default value for testing
    public static string character = "Test";
    public static string pet;
    public static double offset;
}
