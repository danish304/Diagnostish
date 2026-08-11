namespace Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;

 public sealed record RawRamModel(
     string? Type, 
     double? Capacity, 
     int? Speed
 );