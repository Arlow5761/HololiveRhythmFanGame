using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct Grade
{
    public double margin;
    public int score;
}

public class Threshold : MonoBehaviour
{
    public Threshold instance;
    public Grade[] grades;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;

        SortGrades();
    }

    public void SortGrades()
    {
        for (int i = 0; i < grades.Length - 1; i++)
        {
            if (grades[i].margin > grades[i + 1].margin)
            {
                Grade temp = grades[i];
                grades[i] = grades[i + 1];
                grades[i + 1] = temp;
            }
        }
    }

    public Grade GetGrade(double error)
    {
        for (int i = 0; i < grades.Length; i++)
        {
            if (math.abs(error) < grades[i].margin) return grades[i];
        }

        return new Grade { score = 0 };
    }
}
