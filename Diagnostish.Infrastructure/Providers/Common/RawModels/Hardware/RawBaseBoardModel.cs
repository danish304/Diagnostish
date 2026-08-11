namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record RawBaseBoardModel(
    string? Model, 
    string? Manufacturer, 
    string? Version, 
    string? Status
);