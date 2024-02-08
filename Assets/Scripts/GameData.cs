
using System.Collections.Generic;

public static class GameData
{
    public static SongInfo songInfo;
    public static Difficulty selectedDifficulty;
    public static string character = "Test";
    public static string pet;
    public static double offset;

    public static int score = 0;
    public static int combo = 0;
    public static Dictionary<string, int> grades = new() { {"Perfect", 0}, {"Great", 0}, {"Okay", 0}, {"Miss", 0} };
}
