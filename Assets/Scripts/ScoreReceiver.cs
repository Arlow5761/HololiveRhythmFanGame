using System.Xml;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreReceiver : MonoBehaviour
{
    public static ScoreReceiver instance;

    public void Initialize()
    {
        if (instance != null && instance != this) return;

        instance = this;
    }

    public void OnNormalNotePress(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Normal" || grade.name == "Miss") return;

        ScoreManager.instance.IncrementCombo();
        ScoreManager.instance.AddScoreWithCombo(grade.score);
        Scores.grades[grade.name]++;
    }

    public void OnNormalNoteMiss(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Normal" || grade.name != "Miss") return;

        ScoreManager.instance.BreakCombo();
        Scores.grades[grade.name]++;
    }

    public void OnSliderNotePressAndRelease(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Hold" || grade.name == "Miss") return;

        ScoreManager.instance.IncrementCombo();
        ScoreManager.instance.AddScoreWithCombo(grade.score);
        Scores.grades[grade.name]++;
    }

    public void OnSliderNoteTick(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Hold" || grade.name != "SliderTick") return;

        ScoreManager.instance.AddScoreRaw(grade.score);
    }

    public void OnSliderNoteMiss(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Hold" || grade.name != "Miss") return;

        ScoreManager.instance.BreakCombo();
        Scores.grades[grade.name]++;
    }

    public void OnMashNotePress(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Mash" || grade.name == "Miss") return;

        ScoreManager.instance.AddScoreRaw(grade.score);
    }

    public void OnScoreNoteHit(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "ScoreNote") return;

        ScoreManager.instance.AddScoreRaw(grade.score);
    }

    public void OnObstacleHit(NoteData noteData, Grade grade)
    {
        if (noteData.NoteType != "Obstacle") return;

        ScoreManager.instance.BreakCombo();
        Scores.grades[grade.name]++;
    }
}
