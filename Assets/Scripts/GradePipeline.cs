
public class GradePipeline : GenericPipeline<Grade> {}

public class GradePipelineStep : GenericPipelineStep<Grade>
{
    public GradePipelineStep(int priority, GenericPipelineProcessor<Grade> processor) : base(priority, processor) {}
}
