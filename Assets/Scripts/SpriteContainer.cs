using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SpriteElement
{
    public string name;
    public Sprite sprite;

    public static implicit operator Sprite(SpriteElement element) => element.sprite;
    public static implicit operator string(SpriteElement element) => element.name;
}

[Serializable, CreateAssetMenu(fileName = "SpriteContainer", menuName = "Collections/SpriteContainer")]
public class SpriteContainer : ScriptableObject
{
    public SpriteElement[] sprites;

    public Sprite GetSprite(string name)
    {
        for (int i = 0; i < sprites.Count(); i++)
        {
            if (sprites[i] == name)
            {
                return sprites[i];
            }
        }

        return null;
    }
}
