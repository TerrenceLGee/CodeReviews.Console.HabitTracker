using HabitTracker.TerrenceLGee.DTOs;
using HabitTracker.TerrenceLGee.HabitModels;

namespace HabitTracker.TerrenceLGee.Mappings;

public static class FromDto
{
    public static Habit FromHabitDto(this HabitDto dto)
    {
        return new Habit
        {
            Id = dto.Id,
            Name = dto.Name,
            UserId = dto.UserId,
            DateOfOccurrence = dto.DateOfOccurrence,
            UnitOfMeasurement = dto.UnitOfMeasurement,
            Quantity = dto.Quantity,
            Comments = dto.Comments
        };
    }

    public static Habit FromCreateHabitDto(this CreateHabitDto dto, int userId)
    {
        return new Habit
        {
            Name = dto.Name,
            UserId = dto.UserId,
            DateOfOccurrence = dto.DateOfOccurrence,
            UnitOfMeasurement = dto.UnitOfMeasurement,
            Quantity = dto.Quantity,
            Comments = dto.Comments
        };
    }

    public static Habit FromUpdateHabitDto(this UpdateHabitDto dto)
    {
        return new Habit
        {
            Id = dto.Id,
            Name = dto.Name,
            UserId = dto.UserId,
            DateOfOccurrence = dto.DateOfOccurrence,
            UnitOfMeasurement = dto.UnitOfMeasurement,
            Quantity = dto.Quantity,
            Comments = dto.Comments
        };
    }

    public static User FromUserDto(this UserDto dto)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };
    }
}