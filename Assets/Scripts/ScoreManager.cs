using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

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
        GameData.score = score;
        onScoreChanged.Invoke(score);
    }

    public void AddScoreWithCombo(int additionalScore)
    {
        AddScoreRaw(additionalScore * combo);
    }

    public void IncrementCombo()
    {
        combo++;
        if (GameData.combo < combo) GameData.combo = combo;
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
