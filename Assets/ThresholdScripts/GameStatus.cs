using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    [Header("Summary")]
    public int TotalScore;
    public float Accuracy;
    public int Combo;
    public int MaxCombo;

    [Header("Score Breakdown")]
    public int[] CountEachThreshold;

    [Header("Misc")]
    public Threshold LevelThreshold;
    
    public void AddCombo(int AddedCombo = 1)
    {
        Combo += AddedCombo;
    }

    public void ResetCombo()
    {
        Combo = 0;
    }

    public void AddScore(int AddedScore)
    {
        AddRawScore(AddedScore * Combo);
    }

    public void AddRawScore(int AddedScore)
    {
        TotalScore += AddedScore;
    }
}
