using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ResultDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreField;
    [SerializeField] private TextMeshProUGUI comboField;
    [SerializeField] private TextMeshProUGUI[] gradeFields;

    public void DisplayResults()
    {
        scoreField.SetText(GameData.score.ToString());
        comboField.SetText(GameData.combo.ToString());

        for (int i = 0; i < gradeFields.Count(); i++)
        {
            gradeFields[i].SetText(GameData.grades[gradeFields[i].name].ToString());
        }
    }
}
