using HabitTracker.TerrenceLGee.DTOs;
using HabitTracker.TerrenceLGee.HabitModels;

namespace HabitTracker.TerrenceLGee.Mappings;

public static class ToDto
{
    public static HabitDto ToHabitDto(this Habit habit)
    {
        return new HabitDto(
            habit.Id,
            habit.Name,
            habit.UserId,
            habit.DateOfOccurrence,
            habit.UnitOfMeasurement,
            habit.Quantity,
            habit.Comments);
    }

    public static RetrievedUserDto ToRetrievedUserDto(this User user)
    {
        return new RetrievedUserDto(
            user.Id,
            user.FirstName,
            user.LastName);
    }
}