using System.ComponentModel.DataAnnotations;

namespace HabitTracker.TerrenceLGee.HabitTrackerUI.Menus;

public enum MainMenu
{
    [Display(Name = "Add a new habit")]
    AddHabit,
    [Display(Name = "Update an existing habit")]
    UpdateHabit,
    [Display(Name = "Delete an existing habit")]
    DeleteHabit,
    [Display(Name = "View a habit")]
    ViewHabit,
    [Display(Name = "View all habits")]
    ViewHabits,
    [Display(Name = "View habit report")]
    ViewReport,
    [Display(Name = "Exit the program")]
    Exit
}