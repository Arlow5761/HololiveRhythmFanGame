

using UnityEngine;

public class NormalNoteHitHandler : MonoBehaviour
{
    public static NormalNoteHitHandler instance;

    public GradePipeline gradePipeline = new();

    public void Initialize()
    {
        if (instance == null) instance = this;
    }

    public Grade RegisterHit(NoteData noteData, Grade grade)
    {
        Grade processedGrade = gradePipeline.Process(grade);
        Grade finalGrade = processedGrade;

        if (finalGrade.name == "Miss")
        {
            ScoreManager.instance.BreakCombo();
            PlayerController.instance.OnNoteMiss(noteData, grade);
            PlayerController.instance.OnNotePass(noteData, grade);
        }
        else
        {
            ScoreManager.instance.IncrementCombo();
            ScoreManager.instance.AddScoreWithCombo(finalGrade.score);
            PlayerController.instance.OnNotePress(noteData, finalGrade);
            NotesAudioPlayer.instance.OnNormalNotePress(noteData, finalGrade);
        }
        
        Scores.grades[finalGrade.name]++;

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}
