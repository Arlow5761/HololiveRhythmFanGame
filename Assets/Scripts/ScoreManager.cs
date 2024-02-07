using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [HideInInspector] public int score;
    [HideInInspector] public int combo;

    [SerializeField] public UnityEvent<int> onScoreChanged = new();
    [SerializeField] public UnityEvent<int> onComboChanged = new();

    public void AddScoreRaw(int additionalScore)
    {
        score += additionalScore;
        onScoreChanged.Invoke(score);
    }

    public void AddScoreWithCombo(int additionalScore)
    {
        score += additionalScore * combo;
        onScoreChanged.Invoke(score);
    }

    public void IncrementCombo()
    {
        combo++;
        onComboChanged.Invoke(combo);
    }

    public void BreakCombo()
    {
        combo = 0;
        onComboChanged.Invoke(combo);
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
