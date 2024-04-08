
using System.Collections.Generic;

// Class to control what happens when a new base score is added
public class ScorePipeline
{
    private List<ScorePipelineStep> scorePipelineSteps = new();

    public int ProcessScore(int inScore)
    {
        int score = inScore;

        for (int i = 0; i < scorePipelineSteps.Count; i++)
        {
            score = scorePipelineSteps[i].scoreProcessor(score);
        }

        return score;
    }

    public ScorePipelineStep AddStep(ScorePipelineStep step)
    {
        int i = 0;

        for (; i < scorePipelineSteps.Count; i++)
        {
            if (scorePipelineSteps[i].priority < step.priority) break;
        };
        
        scorePipelineSteps.Insert(i, step);

        return step;
    }

    public bool RemoveStep(ScorePipelineStep step)
    {
        return scorePipelineSteps.Remove(step);
    }
}

public delegate int ScoreProcessor(int inScore);

public class ScorePipelineStep
{
    public readonly int priority;
    public readonly ScoreProcessor scoreProcessor;

    public ScorePipelineStep(int newPriority, ScoreProcessor newScoreProcessor)
    {
        priority = newPriority;
        scoreProcessor = newScoreProcessor;
    }
}
