using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB;

    public TMP_Text charaName;
    public SpriteRenderer characterSprite;

    private int selectedIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedIndex"))
        {
            selectedIndex = 0;
        }
        else
        {
            Load();
        }

        UpdateCharacter(selectedIndex);
    }

    public void NextOption()
    {
        selectedIndex++;

        if (selectedIndex >= characterDB.characterCount) selectedIndex = 0;

        UpdateCharacter(selectedIndex);
        Save();
    }

    public void BackOption()
    {
        selectedIndex--;

        if (selectedIndex < 0) selectedIndex = characterDB.characterCount - 1;

        UpdateCharacter(selectedIndex);
        Save();
    }

    private void UpdateCharacter(int selectedIndex)
    {
        Character character = characterDB.GetCharacter(selectedIndex);
        characterSprite.sprite = character.characterSprite;
        charaName.text = character.characterName;
    }

    private void Load()
    {
        selectedIndex = PlayerPrefs.GetInt("selectedIndex");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectedIndex", selectedIndex);
    }
}
