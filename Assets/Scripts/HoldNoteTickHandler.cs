using UnityEngine;

public class HoldNoteTickHandler : MonoBehaviour
{
    public static HoldNoteTickHandler instance;

    public GradePipeline gradePipeline = new();

    public void Initialize()
    {
        if (instance == null) instance = this;
    }

    public Grade RegisterTick(NoteData noteData, Grade grade)
    {
        Grade processedGrade = gradePipeline.Process(grade);
        Grade finalGrade = processedGrade;

        ScoreManager.instance.AddScoreRaw(finalGrade.score);

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}