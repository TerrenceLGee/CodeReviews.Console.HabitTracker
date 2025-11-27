using HabitTracker.TerrenceLGee.HabitModels;

namespace HabitTracker.TerrenceLGee.Data.Interfaces;

public interface IUserRepository
{
    int InsertUser(User user);
    bool UserAlreadyExists(string firstName, string lastName);
    User? GetUser(string firstName, string lastName);
    
}