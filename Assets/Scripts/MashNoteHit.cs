using UnityEngine;

public class MashNoteHitHandler : MonoBehaviour
{
    public static MashNoteHitHandler instance;

    public GradePipeline gradePipeline = new();

    public void Initialize()
    {
        if (instance == null) instance = this;
    }

    public Grade SubmitGrade(Grade grade)
    {
        Grade finalGrade = gradePipeline.Process(grade);

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}