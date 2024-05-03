using UnityEngine;

public class HoldNotePressMissHandler : MonoBehaviour
{
    public static HoldNotePressMissHandler instance;

    public GradePipeline gradePipeline = new();

    public void Initialize()
    {
        if (instance == null) instance = this;
    }

    public Grade RegisterMiss(NoteData noteData, Grade grade)
    {
        Grade processedGrade = gradePipeline.Process(grade);
        Grade finalGrade = processedGrade;

        if (finalGrade.name == "Miss")
        {
            ScoreManager.instance.BreakCombo();
            PlayerController.instance.OnNoteMiss(noteData, finalGrade);
        }
        else
        {
            ScoreManager.instance.IncrementCombo();
            ScoreManager.instance.AddScoreWithCombo(finalGrade.score);
        }

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}