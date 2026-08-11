namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record RawStorageDriveModel(
    string? Model,
    double? Size
);