

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
        Grade processedGrade = gradePipeline.Process(grade);
        Grade finalGrade = processedGrade;

        if (finalGrade.name == "Miss")
        {
        }
        else
        {
            PlayerController.instance.OnNotePass(noteData, finalGrade);
        }

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}
