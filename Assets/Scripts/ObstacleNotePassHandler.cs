

using UnityEngine;

public class ObstacleNotePassHandler : MonoBehaviour
{
    public static ObstacleNotePassHandler instance;

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
            ScoreManager.instance.BreakCombo();
        }

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}
