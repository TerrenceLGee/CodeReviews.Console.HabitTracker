using HabitTracker.TerrenceLGee.HabitModels;

namespace HabitTracker.TerrenceLGee.Data.Interfaces;

public interface IHabitRepository
{
    int Insert(Habit habit);
    int Update(Habit habit);
    int Delete(int userId, int habitId);
    bool HabitExists(int userId, int habitId);
    bool AnyHabitsExistForUser(int userId);
    Habit? GetHabitForUser(int userId, int habitId);
    List<Habit> GetHabitsForUser(int userId);
    List<string> GetHabitNamesForUser(int userId);
}