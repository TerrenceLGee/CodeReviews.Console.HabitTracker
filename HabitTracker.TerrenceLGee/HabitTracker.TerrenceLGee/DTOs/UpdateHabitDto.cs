using HabitTracker.TerrenceLGee.HabitModels;

namespace HabitTracker.TerrenceLGee.DTOs;

public record UpdateHabitDto(
    int Id,
    string Name,
    int UserId,
    DateOnly DateOfOccurrence,
    UnitOfMeasurement UnitOfMeasurement,
    int Quantity,
    string? Comments);