using HabitTracker.TerrenceLGee.DTOs;
using HabitTracker.TerrenceLGee.Extensions;
using HabitTracker.TerrenceLGee.HabitModels;
using HabitTracker.TerrenceLGee.HabitTrackerUI.Menus;
using Spectre.Console;

namespace HabitTracker.TerrenceLGee.HabitTrackerUI.Helpers;

public static class HabitHelpers
{
    public static void PressAnyKeyToContinue(
        string consoleColor,
        string message)
    {
        AnsiConsole.MarkupLine($"[{consoleColor}]{message}[/]");
        AnsiConsole.MarkupLine("[steelblue1_1]Press any key to continue[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }
    
    public static DateOnly GetDate(string dateFormat, string consoleColor = "springgreen1")
    {
        DateOnly validDate;

        var dateString = AnsiConsole
            .Ask<string>($"[{consoleColor}]Enter date for the habit in format: {dateFormat}: [/]");
        
        while (!DateOnly.TryParseExact(dateString, dateFormat, out validDate))
        {
            dateString = AnsiConsole
                .Ask<string>($"[bold red]\nInvalid date. Please enter date in format {dateFormat}: [/]");
            while (!IsValidDate(validDate)) ;
        }

        return validDate;
    }

    public static UnitOfMeasurement GetUnitOfMeasurement()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<UnitOfMeasurement>()
                .Title("[aquamarine1]Please choose the unit of measurement for your habit\n[/]")
                .AddChoices(Enum.GetValues<UnitOfMeasurement>())
                .UseConverter(choice => choice.GetDisplayName()));
    }

    public static MainMenu GetMainMenuChoice()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<MainMenu>()
                .Title("[aquamarine1]Please choose one of the following options\n[/]")
                .AddChoices(Enum.GetValues<MainMenu>())
                .UseConverter(choice => choice.GetDisplayName()));
    }
    
    public static int GetHabitIdFromDisplay(List<HabitDto> habits, string purpose)
    {
        var selectedHabit = AnsiConsole.Prompt(
            new SelectionPrompt<HabitDto>()
                .Title($"[darkolivegreen1]Please choose a habit to {purpose}[/]")
                .AddChoices(habits));

        return selectedHabit.Id;
    }

    public static string GetHabitName(List<string> habitNames, string purpose)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[skyblue3]Here are your previously tracked habits. " +
                       $"Please choose the one you wish {purpose}[/]")
                .AddChoices(habitNames));
    }

    public static bool GetOptionalInput(string message, string consoleColor = "springgreen1")
    {
        return AnsiConsole.Confirm($"[{consoleColor}]{message}[/]");
    }

    private static bool IsLeapYear(int year)
    {
        return year % 400 == 0 || (year % 4 == 0 && year % 100 != 0);
    }

    private static bool IsValidDate(DateOnly date)
    {
        var year = date.Year;
        var month = date.Month;
        var dayOfMonth = date.Day;
        var daysInEachMonth = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        var isYearLeap = IsLeapYear(year);
        if (isYearLeap)
        {
            daysInEachMonth[1]++;
        }

        if (month < 1 || month > 12)
        {
            return false;
        }

        if (dayOfMonth < 1 || dayOfMonth > daysInEachMonth[month - 1])
        {
            return false;
        }

        return true;
    }
}