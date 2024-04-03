using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInitializer : MonoBehaviour
{
    public static CharacterInitializer instance;

    public void Initialize()
    {
        if (instance != this && instance != null) return;
        instance = this;
    }

    public void SetupCharacter()
    {
        CharacterDictionary.GetCharacter(GameData.character).Setup();
    }

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        SetupCharacter();
    }
}
