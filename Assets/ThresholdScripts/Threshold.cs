using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Threshold : MonoBehaviour
{
    [Serializable] public struct ScoreRange
    {
        public double Range;
        public int Score;
    }

    public ScoreRange[] Thresholds;
    public int SliderTickScore;
    public int BeatdownScore;

    private double MaximumError;

    private void UpdateMaximumError()
    {
        MaximumError = Thresholds.Last().Range;
    }

    public double GetMaximumError()
    {
        return MaximumError;
    }
}
