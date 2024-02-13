using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Class to display current score
public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreField;
    
    public void OnScoreChanged(int newScore)
    {
        scoreField.SetText(newScore.ToString());
    }
}
