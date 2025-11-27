using HabitTracker.TerrenceLGee.Data.Interfaces;
using HabitTracker.TerrenceLGee.DTOs;
using HabitTracker.TerrenceLGee.Mappings;
using HabitTracker.TerrenceLGee.Services.Interfaces;

namespace HabitTracker.TerrenceLGee.Services;

public class HabitService : IHabitService
{
    private readonly IHabitRepository _repository;

    public HabitService(IHabitRepository repository)
    {
        _repository = repository;
    }

    public int AddHabit(CreateHabitDto dto)
    {
        return _repository.Insert(dto.FromCreateHabitDto(dto.UserId));
    }

    public int UpdateHabit(UpdateHabitDto dto)
    {
        return _repository.Update(dto.FromUpdateHabitDto());
    }

    public int DeleteHabit(int userId, int habitId)
    {
        return _repository.Delete(userId, habitId);
    }

    public bool HabitExists(int userId, int habitId)
    {
        return _repository.HabitExists(userId, habitId);
    }

    public bool AnyHabitsExistForUser(int userId)
    {
        return _repository.AnyHabitsExistForUser(userId);
    }

    public HabitDto? GetHabitForUser(int userId, int habitId)
    {
        var habit = _repository.GetHabitForUser(userId, habitId);
        return habit?.ToHabitDto();
    }

    public List<HabitDto> GetHabitsForUser(int userId)
    {
        return _repository.GetHabitsForUser(userId)
            .Select(h => h.ToHabitDto())
            .ToList();
    }
    
    public List<string> GetHabitNamesForUser(int userId)
    {
        return _repository.GetHabitNamesForUser(userId);
    }
}