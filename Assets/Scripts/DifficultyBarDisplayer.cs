using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyBarDisplayer : MonoBehaviour
{
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI ratingField;
    public TextMeshProUGUI gradeField;

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChangeDifficulty(string name, float rating, string grade)
    {
        nameField.SetText(name);
        ratingField.SetText(rating.ToString());
        gradeField.SetText(grade);
    }
}
