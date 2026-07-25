namespace Diagnostish.Application.Pipelines;

public sealed record ComponentPipeline<TReport>(Action<TReport> Run);