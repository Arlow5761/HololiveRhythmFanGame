using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct Grade
{
    public string name;
    public double margin;
    public int score;

    public static Grade Null = new(){ name = "Null", margin = 0, score = 0 };
}

// Singleton class that handles the score thresholds on a level
public class Threshold : MonoBehaviour
{
    public static Threshold instance;
    public Grade[] grades;
    public Grade[] specialGrades;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void SortGrades()
    {
        for (int j = 0; j < grades.Length - 1; j++)
        {
            for (int i = 0; i < grades.Length - 1 - j; i++)
            {
                if (grades[i].margin > grades[i + 1].margin)
                {
                    Grade temp = grades[i];
                    grades[i] = grades[i + 1];
                    grades[i + 1] = temp;
                }
            }
        }
    }

    public Grade GetGrade(double error)
    {
        for (int i = 0; i < grades.Length; i++)
        {
            if (math.abs(error) < grades[i].margin)
            {
                return grades[i];
            }
        }

        return Array.Find(specialGrades, grade => grade.name == "Miss");
    }

    public Grade GetSpecialGrade(string name)
    {
        return Array.Find(specialGrades, grade => grade.name == name);
    }
}
