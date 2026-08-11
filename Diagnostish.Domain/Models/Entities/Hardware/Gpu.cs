namespace Diagnostish.Domain.Models.Entities.Hardware;

public sealed record Gpu(
    string Name, 
    double AdapterRam
);
