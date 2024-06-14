using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterDictionary
{
    private static Dictionary<string, CharacterInterface> characters = new()
    {
        {"Test", new TestCharacter()},
        {"Mori", new MoriCharacter()}
    };

    public static CharacterInterface GetCharacter(string characterName)
    {
        return characters.GetValueOrDefault(characterName, null);
    }
}
