namespace Application.Pipelines;

public sealed record ComponentPipeline<TReport>(
    Func<CancellationToken, Task<Action<TReport>>> CollectAndAnalyze);