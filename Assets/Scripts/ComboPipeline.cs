


public class ComboPipeline : GenericPipeline<int> {}

public class ComboPipelineStep : GenericPipelineStep<int>
{
    public ComboPipelineStep(int priority, GenericPipelineProcessor<int> processor) : base(priority, processor) {}
}