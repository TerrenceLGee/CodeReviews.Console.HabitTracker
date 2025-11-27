using HabitTracker.TerrenceLGee.DTOs;

namespace HabitTracker.TerrenceLGee.HabitTrackerUI.Interfaces;

public interface ITrackerOperationsUi
{
    RetrievedUserDto? AddUser();
    void AddHabit(RetrievedUserDto dto);
    void UpdateHabit(RetrievedUserDto dto);
    void DeleteHabit(RetrievedUserDto dto);
    void ViewHabit(RetrievedUserDto dto);
    bool ViewHabits(RetrievedUserDto dto);
    void ViewHabitReport(RetrievedUserDto dto);
}