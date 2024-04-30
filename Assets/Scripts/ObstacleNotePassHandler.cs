

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
