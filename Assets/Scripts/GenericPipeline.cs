using System.Collections.Generic;

// Generic pipeline class for sequential processing of data
public class GenericPipeline<T>
{
    private List<GenericPipelineStep<T>> steps = new();

    public virtual T Process(T input)
    {
        T intermediate = input;

        for (int i = 0; i < steps.Count; i++)
        {
            intermediate = steps[i].processor(intermediate);
        }

        return intermediate;
    }

    public virtual GenericPipelineStep<T> AddStep(GenericPipelineStep<T> step)
    {
        int i = 0;

        for (; i < steps.Count; i++)
        {
            if (steps[i].priority < step.priority) break;
        };
        
        steps.Insert(i, step);

        return step;
    }

    public virtual bool RemoveStep(GenericPipelineStep<T> step)
    {
        return steps.Remove(step);
    }
}

// Generic pipeline step class to store a single process in a pipeline
public class GenericPipelineStep<T>
{
    public readonly int priority;
    public readonly GenericPipelineProcessor<T> processor;

    public GenericPipelineStep(int priority, GenericPipelineProcessor<T> processor)
    {
        this.priority = priority;
        this.processor = processor;
    }
}

// Delegate to store a function as a process in a pipeline step
public delegate T GenericPipelineProcessor<T>(T input);
