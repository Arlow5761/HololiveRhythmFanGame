


public class ScorePipeline : GenericPipeline<int> {}

public class ScorePipelineStep : GenericPipelineStep<int>
{
    public ScorePipelineStep(int priority, GenericPipelineProcessor<int> processor) : base(priority, processor) {}
}
