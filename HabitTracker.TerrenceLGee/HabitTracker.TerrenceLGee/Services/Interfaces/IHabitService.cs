using HabitTracker.TerrenceLGee.DTOs;

namespace HabitTracker.TerrenceLGee.Services.Interfaces;

public interface IHabitService
{
    int AddHabit(CreateHabitDto dto);
    int UpdateHabit(UpdateHabitDto dto);
    int DeleteHabit(int userId, int habitId);
    bool HabitExists(int userId, int habitId);
    bool AnyHabitsExistForUser(int userId);
    HabitDto? GetHabitForUser(int userId, int habitId);
    List<HabitDto> GetHabitsForUser(int userId);
    List<string> GetHabitNamesForUser(int userId);
}