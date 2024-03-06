// Static class to store general game data
using UnityEditor;

public static class GameData
{
    public static SongInfo songInfo = new() // Default value for testing
    {
        metadata = new()
        {
            songName = "Shiny Smily Story (2022 ver.)",
            artist = "holoJP",
            bpm = "163",
            difficulties = new Difficulty[]
            {
                new()
                {
                    name = "Standard",
                    rating = 7,
                    notePath = "Songs//ShinySmilyStory//Standard.json"
                }
            },
            songPath = "Songs//ShinySmilyStory//ShinySmilyStory.ogg",
            coverPath = "Songs//ShinySmilyStory//background.jpg"
        }
    };
    public static Difficulty selectedDifficulty = songInfo.metadata.difficulties[0]; // Default value for testing
    public static string character = "Test";
    public static string pet;
    public static double offset;
}
