using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PetDatabase : ScriptableObject
{
    public Pet[] pet;

    public int petCount
    {
        get
        {
            return pet.Length;
        }
    }

    public Pet GetPet(int index)
    {
        return pet[index];
    }
}
