using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

// Singleton class that handles score, combo, and fever
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [HideInInspector] public int score;
    [HideInInspector] public int combo;

    public float feverMultBonus;
    public int feverBonus;

    [HideInInspector] public float multiplier = 1;
    [HideInInspector] public int bonus = 0;

    [SerializeField] public UnityEvent<int> onScoreChanged = new();
    [SerializeField] public UnityEvent<int> onComboChanged = new();

    public void AddScoreRaw(int additionalScore)
    {
        score += (int) (additionalScore * math.max(0, multiplier) + math.max(0, bonus));
        Scores.score = score;
        onScoreChanged.Invoke(score);
    }

    public void AddScoreWithCombo(int additionalScore)
    {
        AddScoreRaw(additionalScore * combo);
    }

    public void IncrementCombo()
    {
        combo++;
        if (Scores.combo < combo) Scores.combo = combo;
        onComboChanged.Invoke(combo);
    }

    public void BreakCombo()
    {
        combo = 0;
        Scores.fullCombo = false;
        onComboChanged.Invoke(combo);
    }

    public void Initialize()
    {
        if (instance != this && instance != null) return;

        instance = this;
    }

    public void OnFeverStarted()
    {
        multiplier += feverMultBonus;
        bonus += feverBonus;
    }

    public void OnFeverEnded()
    {
        multiplier -= feverMultBonus;
        bonus -= feverBonus;
    }

    void Awake()
    {
        Initialize();
    }
}
