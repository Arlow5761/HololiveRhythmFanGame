using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PetManager : MonoBehaviour
{
    public PetDatabase petDB;

    public TMP_Text petName;
    public SpriteRenderer petSprite;

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

        if (selectedIndex >= petDB.petCount) selectedIndex = 0;

        UpdateCharacter(selectedIndex);
        Save();
    }

    public void BackOption()
    {
        selectedIndex--;

        if (selectedIndex < 0) selectedIndex = petDB.petCount - 1;

        UpdateCharacter(selectedIndex);
        Save();
    }

    private void UpdateCharacter(int selectedIndex)
    {
        Pet pet = petDB.GetPet(selectedIndex);
        petSprite.sprite = pet.petSprite;
        petName.text = pet.petName;
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
