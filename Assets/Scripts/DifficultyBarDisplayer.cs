using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyBarDisplayer : MonoBehaviour
{
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI ratingField;
    public Image gradeField;
    public Button button;

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChangeDifficulty(Difficulty difficulty, Sprite gradeSprite)
    {
        button.onClick.RemoveAllListeners();

        nameField.SetText(difficulty.name);
        ratingField.SetText(difficulty.rating.ToString());
        gradeField.sprite = gradeSprite;

        if (gradeSprite == null)
        {
            gradeField.color = new(255, 255, 255, 0);
        }
        else
        {
            gradeField.color = new(255, 255, 255, 1);
        }

        button.onClick.AddListener(() => {
            GameData.selectedDifficulty = difficulty;
            SceneHandler.instance.LoadScene("GameplayScene");
        });
    }
}
