using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyBarDisplayer : MonoBehaviour
{
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI ratingField;
    public TextMeshProUGUI gradeField;
    public Button button;

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChangeDifficulty(Difficulty difficulty)
    {
        button.onClick.RemoveAllListeners();
        nameField.SetText(difficulty.name);
        ratingField.SetText(difficulty.rating.ToString());
        gradeField.SetText("S");
        button.onClick.AddListener(() => {
            GameData.selectedDifficulty = difficulty;
            SceneHandler.instance.LoadScene("GameplayScene");
        });
    }
}
