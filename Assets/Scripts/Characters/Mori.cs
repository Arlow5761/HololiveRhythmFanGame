using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoriCharacter : CharacterInterface
{
    int livesCount = 0;
    int currentCombo = 0;

    public void Setup()
    {
        livesCount = 0;

        ScoreManager.instance.comboPipeline.AddStep(new(-1, (int combo) => {
            if (livesCount > 0 && combo == 0)
            {
                livesCount -= 1;
                combo = currentCombo;
                Debug.Log("Current Lives: " + livesCount);
            }

            if (combo > 0 && combo % 50 == 0 && livesCount < 3)
            {
                livesCount += 1;
                Debug.Log("Current Lives: " + livesCount);
            }

            currentCombo = combo;

            return combo;
        }));

        Debug.Log("Mori Initialized");
    }
}
