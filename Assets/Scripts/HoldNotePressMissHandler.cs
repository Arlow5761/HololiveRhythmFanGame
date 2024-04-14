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

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}