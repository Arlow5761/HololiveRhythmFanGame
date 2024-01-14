using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Purchasing;

public class ProcessInput : MonoBehaviour
{
    public Timeline LevelTimeline;
    public GameStatus LevelStatus;
    public Threshold LevelThreshold;

    private int[] HeldSliders = {0, 0};
    private int[] HeldButtons = {0, 0};

    void Awake()
    {
        Array.Resize(ref HeldSliders, 2);
        Array.Resize(ref HeldButtons, 2);
    }

    public void Down(int Lane)
    {
        HeldButtons[Lane]++;

        Timeline.HitObject NextHitObject = LevelTimeline.GetCurrentHitObject(Lane);

        switch (NextHitObject.Type)
        {
            case Timeline.HitObject.HitObjectType.Press:
                Press(Lane);
            break;
            case Timeline.HitObject.HitObjectType.SliderStart:
                SliderStart(Lane);
            break;
            case Timeline.HitObject.HitObjectType.BeatdownStart:
                BeatdownStart(Lane);
            break;
            case Timeline.HitObject.HitObjectType.BeatdownEnd:
                BeatdownEnd(Lane);
            break;
        }
    }

    public void Release(int Lane)
    {
        HeldButtons[Lane]--;

        Timeline.HitObject NextHitObject = LevelTimeline.GetCurrentHitObject(Lane);

        switch (NextHitObject.Type)
        {
            case Timeline.HitObject.HitObjectType.SliderEnd:
                SliderEnd(Lane);
            break;
        }

        if (HeldButtons[Lane] < HeldSliders[Lane])
        {
            LevelStatus.ResetCombo();
            ReleaseSlider(Lane);
        }
    }

    public void Miss(int Lane)
    {
        Timeline.HitObject NextHitObject = LevelTimeline.GetCurrentHitObject(Lane);

        switch (NextHitObject.Type)
        {
            case Timeline.HitObject.HitObjectType.SliderEnd:
                if (LevelTimeline.GetHitObjectAt(Lane, NextHitObject.StartingPoint).SliderStarted)
                {
                    LevelStatus.ResetCombo();
                    ReleaseSlider(Lane);
                }
            break;
            case Timeline.HitObject.HitObjectType.BeatdownStart:
                BeatdownStart(Lane);
            break;
            case Timeline.HitObject.HitObjectType.BeatdownEnd:
                if (LevelTimeline.GetHitObjectAt(Lane, NextHitObject.StartingPoint).SliderStarted)
                {
                    LevelStatus.ResetCombo();
                }
            break;
            default:
                LevelStatus.ResetCombo();
            break;
        }
    }

    private void Press(int Lane)
    {
        CalculateScore(Lane);
    }

    private void SliderStart(int Lane)
    {
        int Result = CalculateScore(Lane);

        if (Result == 0) return;

        HoldSlider(Lane);
        LevelTimeline.GetHitObjectAt(Lane, LevelTimeline.Lanes[Lane].Pointer - 1).SliderStarted = true;
    }

    private void SliderEnd(int Lane)
    {
        Timeline.HitObject NextHitObject = LevelTimeline.GetCurrentHitObject(Lane);

        if (!LevelTimeline.GetHitObjectAt(Lane, NextHitObject.StartingPoint).SliderStarted) return;

        int Result = CalculateScore(Lane);

        if (Result == 0) return;

        ReleaseSlider(Lane);
    }

    private void BeatdownStart(int Lane)
    {
        
    }

    private void BeatdownEnd(int Lane)
    {
        Timeline.HitObject NextHitObject = LevelTimeline.GetCurrentHitObject(Lane);

        LevelTimeline.GetHitObjectAt(Lane, NextHitObject.StartingPoint).SliderStarted = true;

        LevelStatus.AddRawScore(LevelThreshold.BeatdownScore);
    }

    private int CalculateScore(int Lane)
    {
        Timeline.HitObject NextHitObject = LevelTimeline.GetCurrentHitObject(Lane);

        for (int i = 0; i < LevelThreshold.Thresholds.Length; i++)
        {
            if (math.abs(NextHitObject.Timepoint - LevelTimeline.CurrentTime) < LevelThreshold.Thresholds[i].Range)
            {
                LevelStatus.AddCombo();
                LevelStatus.AddScore(LevelThreshold.Thresholds[i].Score);
                LevelStatus.CountEachThreshold[i]++;

                LevelTimeline.ShiftPointer(Lane);

                return LevelThreshold.Thresholds[i].Score;
            }
        }

        return 0;
    }

    private void HoldSlider(int Lane)
    {
        HeldSliders[Lane]++;
    }

    private void ReleaseSlider(int Lane)
    {
        HeldSliders[Lane]--;
    }
}
