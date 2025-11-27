using HabitTracker.TerrenceLGee.HabitTrackerUI.Helpers;
using HabitTracker.TerrenceLGee.HabitTrackerUI.Interfaces;
using HabitTracker.TerrenceLGee.HabitTrackerUI.Menus;
using Spectre.Console;

namespace HabitTracker.TerrenceLGee.HabitTrackerUI;

public class HabitTrackerApp
{
    private readonly ITrackerOperationsUi _trackerUi;

    public HabitTrackerApp(ITrackerOperationsUi trackerUi)
    {
        _trackerUi = trackerUi;
    }

    public void Run()
    {
        AnsiConsole.MarkupLine("[honeydew2]Welcome to the Habit Tracker\n[/]");
        var user = _trackerUi.AddUser();

        if (user is null)
        {
            HabitHelpers.PressAnyKeyToContinue("bold red", "\nThere was an error running this program");
            return;
        }
        else
        {
            HabitHelpers.PressAnyKeyToContinue("honeydew2", "Let's begin!");
        }

        var userFinished = false;

        while (!userFinished)
        {
            var choice = HabitHelpers.GetMainMenuChoice();

            switch (choice)
            {
                case MainMenu.AddHabit:
                    _trackerUi.AddHabit(user);
                    break;
                case MainMenu.UpdateHabit:
                    _trackerUi.UpdateHabit(user);
                    break;
                case MainMenu.DeleteHabit:
                    _trackerUi.DeleteHabit(user);
                    break;
                case MainMenu.ViewHabit:
                    _trackerUi.ViewHabit(user);
                    break;
                case MainMenu.ViewHabits:
                    _trackerUi.ViewHabits(user);
                    break;
                case MainMenu.ViewReport:
                    _trackerUi.ViewHabitReport(user);
                    break;
                case MainMenu.Exit:
                    userFinished = true;
                    break;
                default:
                    HabitHelpers
                        .PressAnyKeyToContinue("bold red", "Invalid choice please try again");
                    break;
            }
        }
    }
}