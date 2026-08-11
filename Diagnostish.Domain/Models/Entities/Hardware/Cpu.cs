namespace Diagnostish.Domain.Models.Entities.Hardware;

public sealed record Cpu(
    string Name,
    int Cores,
    int ClockSpeed
);