using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score;
    public int combo;

    public void AddScoreRaw(int additionalScore)
    {
        score += additionalScore;
    }

    public void AddScoreWithCombo(int additionalScore)
    {
        score += additionalScore * combo;
        Debug.Log(score);
    }

    public void IncrementCombo()
    {
        combo++;
        Debug.Log(combo);
    }

    public void BreakCombo()
    {
        combo = 0;
        Debug.Log(combo);
    }

    public void Initialize()
    {
        if (instance != this && instance != null) return;

        instance = this;
    }

    void Awake()
    {
        Initialize();
    }
}
