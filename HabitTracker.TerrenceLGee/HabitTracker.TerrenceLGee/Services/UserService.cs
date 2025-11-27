using HabitTracker.TerrenceLGee.Data.Interfaces;
using HabitTracker.TerrenceLGee.DTOs;
using HabitTracker.TerrenceLGee.Mappings;
using HabitTracker.TerrenceLGee.Services.Interfaces;

namespace HabitTracker.TerrenceLGee.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public int CreateUser(UserDto dto)
    {
        return _repository.InsertUser(dto.FromUserDto());
    }

    public bool UserAlreadyExists(UserDto dto)
    {
        return _repository.UserAlreadyExists(dto.FirstName, dto.LastName);
    }

    public RetrievedUserDto? GetUser(UserDto dto)
    {
        var user = _repository.GetUser(dto.FirstName, dto.LastName);
        return user?.ToRetrievedUserDto();
    }
}