using UnityEngine;

public class HoldNoteReleaseHandler : MonoBehaviour
{
    public static HoldNoteReleaseHandler instance;

    public GradePipeline gradePipeline = new();

    public void Initialize()
    {
        if (instance == null) instance = this;
    }

    public Grade RegisterRelease(NoteData noteData, Grade grade)
    {
        Grade processedGrade = gradePipeline.Process(grade);
        Grade finalGrade = processedGrade;

        if (finalGrade.name == "Miss")
        {
            ScoreManager.instance.BreakCombo();
            PlayerController.instance.OnNoteMiss(noteData, grade);
        }
        else
        {
            ScoreManager.instance.IncrementCombo();
            ScoreManager.instance.AddScoreWithCombo(finalGrade.score);
        }
        
        Scores.grades[finalGrade.name]++;

        PlayerController.instance.OnNoteRelease(noteData, finalGrade);

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}