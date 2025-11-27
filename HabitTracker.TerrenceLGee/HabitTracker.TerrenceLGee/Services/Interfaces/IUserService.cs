using HabitTracker.TerrenceLGee.DTOs;

namespace HabitTracker.TerrenceLGee.Services.Interfaces;

public interface IUserService
{
    int CreateUser(UserDto dto);
    bool UserAlreadyExists(UserDto dto);
    RetrievedUserDto? GetUser(UserDto dto);
    
}