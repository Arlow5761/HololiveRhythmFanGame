using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ResultDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreField;
    [SerializeField] private TextMeshProUGUI comboField;
    [SerializeField] private TextMeshProUGUI accuracyField;
    [SerializeField] private Image finalGradeImage;
    [SerializeField] private TextMeshProUGUI[] gradeFields;
    [SerializeField] private SpriteContainer finalGradeSprites;

    public void OnEnterScene()
    {
        Scores.CalculateAccuracy();
        Scores.GetFinalGrade();

        Scores.SaveScores();

        DisplayResults();
    }

    public void DisplayResults()
    {
        scoreField.SetText(Scores.score.ToString());
        comboField.SetText(Scores.combo.ToString());
        accuracyField.SetText(string.Format("{0:0.00}%", Scores.accuracy));

        for (int i = 0; i < gradeFields.Count(); i++)
        {
            gradeFields[i].SetText(Scores.grades[gradeFields[i].name].ToString());
        }

        finalGradeImage.sprite = finalGradeSprites.GetSprite(Scores.finalGrade);
    }
}
