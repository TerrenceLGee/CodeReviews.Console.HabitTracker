using HabitTracker.TerrenceLGee.HabitModels;

namespace HabitTracker.TerrenceLGee.DTOs;

public record CreateHabitDto(
    string Name,
    int UserId,
    DateOnly DateOfOccurrence,
    UnitOfMeasurement UnitOfMeasurement,
    int Quantity,
    string? Comments);