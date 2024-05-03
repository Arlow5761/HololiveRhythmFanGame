using UnityEngine;

public class HoldNoteReleaseMissHandler : MonoBehaviour
{
    public static HoldNoteReleaseMissHandler instance;

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

        PlayerController.instance.OnNoteRelease(noteData, finalGrade);

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}