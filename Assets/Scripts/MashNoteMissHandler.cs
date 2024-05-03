using UnityEngine;

public class MashNoteMissHandler : MonoBehaviour
{
    public static MashNoteMissHandler instance;

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
            Scores.grades[finalGrade.name]++;
        }
        else
        {
            ScoreManager.instance.AddScoreRaw(finalGrade.score);
        }

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}