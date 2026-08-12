namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;

public sealed record StorageDriveRawModel(
    string? Model,
    double? Size
);