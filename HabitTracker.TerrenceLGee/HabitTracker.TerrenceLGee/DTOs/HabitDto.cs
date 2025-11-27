using HabitTracker.TerrenceLGee.HabitModels;

namespace HabitTracker.TerrenceLGee.DTOs;

public record HabitDto(
    int Id,
    string Name,
    int UserId,
    DateOnly DateOfOccurrence,
    UnitOfMeasurement UnitOfMeasurement,
    int Quantity,
    string? Comments)
{
    public override string ToString()
    {
        return $"{Id}\t{Name}\t{DateOfOccurrence.ToString()}";
    }
}