

using UnityEngine;

public class ScoreNotePassHandler : MonoBehaviour
{
    public static ScoreNotePassHandler instance;

    public GradePipeline gradePipeline = new();

    public void Initialize()
    {
        if (instance == null) instance = this;
    }

    public Grade RegisterPass(NoteData noteData, Grade grade)
    {
        Grade finalGrade = grade;

        if (PlayerController.instance.IsCollidingWithNote(noteData))
        {
            Grade processedGrade = gradePipeline.Process(grade);
            finalGrade = processedGrade;
        }
        else
        {
            finalGrade = Threshold.instance.GetSpecialGrade("Miss");
        }

        if (finalGrade.name == "Miss")
        {
        }
        else
        {
            PlayerController.instance.OnNotePass(noteData, finalGrade);
            ScoreManager.instance.AddScoreRaw(finalGrade.score);
        }

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}
