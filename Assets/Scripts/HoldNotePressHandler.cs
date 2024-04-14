using UnityEngine;

public class HoldNotePressHandler : MonoBehaviour
{
    public static HoldNotePressHandler instance;

    public GradePipeline gradePipeline = new();

    public void Initialize()
    {
        if (instance == null) instance = this;
    }

    public Grade RegisterPress(NoteData noteData, Grade grade)
    {
        Grade processedGrade = gradePipeline.Process(grade);
        Grade finalGrade = processedGrade;

        if (finalGrade.name == "Miss")
        {
            ScoreManager.instance.BreakCombo();
        }
        else
        {
            ScoreManager.instance.IncrementCombo();
            ScoreManager.instance.AddScoreWithCombo(finalGrade.score);
        }
        
        Scores.grades[finalGrade.name]++;

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}