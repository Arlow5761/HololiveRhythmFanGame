using UnityEngine;

public class MashNoteHitHandler : MonoBehaviour
{
    public static MashNoteHitHandler instance;

    public GradePipeline gradePipeline = new();

    public void Initialize()
    {
        if (instance == null) instance = this;
    }

    public Grade RegisterHit(NoteData noteData, Grade grade)
    {
        Grade processedGrade = gradePipeline.Process(grade);
        Grade finalGrade = processedGrade;

        ScoreManager.instance.AddScoreRaw(finalGrade.score);
        PlayerController.instance.OnNotePress(noteData, finalGrade);
        NotesAudioPlayer.instance.OnMashNotePress(noteData, finalGrade);

        return finalGrade;
    }

    void Awake()
    {
        Initialize();
    }
}