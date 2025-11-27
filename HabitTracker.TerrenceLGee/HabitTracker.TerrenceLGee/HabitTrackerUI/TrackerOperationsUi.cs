using System.Text;
using HabitTracker.TerrenceLGee.DTOs;
using HabitTracker.TerrenceLGee.Extensions;
using HabitTracker.TerrenceLGee.HabitModels;
using HabitTracker.TerrenceLGee.HabitTrackerUI.Helpers;
using HabitTracker.TerrenceLGee.HabitTrackerUI.Interfaces;
using HabitTracker.TerrenceLGee.Services.Interfaces;
using Spectre.Console;

namespace HabitTracker.TerrenceLGee.HabitTrackerUI;

public class TrackerOperationsUi : ITrackerOperationsUi
{
    private readonly IUserService _userService;
    private readonly IHabitService _habitService;
    private const string DateFormat = "MM-dd-yyyy";

    public TrackerOperationsUi(
        IUserService userService,
        IHabitService habitService)
    {
        _userService = userService;
        _habitService = habitService;
    }


    public RetrievedUserDto? AddUser()
    {
        var firstName = AnsiConsole
            .Ask<string>("[cyan2]Please enter your first name: [/]")
            .Trim();
        var lastName = AnsiConsole
            .Ask<string>("[cyan2]Please enter your last name: [/]")
            .Trim();

        var userDto = new UserDto(firstName, lastName);

        var userAlreadyExists = _userService.UserAlreadyExists(userDto);

        if (!userAlreadyExists)
        {
            _userService.CreateUser(userDto);
        }

        var user = _userService.GetUser(userDto);

        if (user is not null)
        {
            var welcomeMessage = userAlreadyExists
                ? " back"
                : "";

            var messageToDisplay = $"\nGood to have you{welcomeMessage} {user.FirstName} {user.LastName}!";

            AnsiConsole.MarkupLine($"[cyan2]{messageToDisplay}[/]");

            return user;
        }

        return null;
    }

    public void AddHabit(RetrievedUserDto dto)
    {
        var isAlreadyTracked = AnsiConsole
            .Confirm("[skyblue3]Will you be adding a previously tracked habit? [/]");

        if (!_habitService.AnyHabitsExistForUser(dto.Id))
        {
            isAlreadyTracked = false;
            AnsiConsole.MarkupLine(
                $"[chartreuse2]{dto.FirstName} it looks like you don't have any previously tracked habits[/]");
            AnsiConsole.MarkupLine("[chartreuse2]So let's add a new habit instead![/]");
        }

        string habitName;
        var habitNames = _habitService.GetHabitNamesForUser(dto.Id);

        if (isAlreadyTracked)
        {
            habitName = HabitHelpers.GetHabitName(habitNames, "to track again");
        }
        else
        {
            habitName = AnsiConsole.Ask<string>("[skyblue3]Enter the name of the habit to track: [/]")
                .Trim();
            habitName = PreventHabitNameFromChangingCase(habitNames, habitName);
        }

        var dateOfOccurrence = HabitHelpers.GetDate(DateFormat);

        var unitOfMeasurement = HabitHelpers.GetUnitOfMeasurement();

        var quantity = AnsiConsole
            .Ask<int>($"[skyblue3]Enter the quantity of {unitOfMeasurement.GetDisplayName()} for the habit: [/]");

        string? comments = null;

        if (HabitHelpers.GetOptionalInput("Do you wish to add comments related to your habit? "))
        {
            comments = AnsiConsole.Ask<string>("[skyblue3]Enter your comments: [/]")
                .Trim();
        }

        var createdHabit = new CreateHabitDto(
            habitName,
            dto.Id,
            dateOfOccurrence,
            unitOfMeasurement,
            quantity,
            comments);

        HabitHelpers.PressAnyKeyToContinue("skyblue3",
            _habitService.AddHabit(createdHabit) == 1
                ? $"{dto.FirstName} habit was tracked successfully!"
                : $"Unfortunately {dto.FirstName} there was a problem tracking your habit");
    }

    public void UpdateHabit(RetrievedUserDto dto)
    {
        if (!_habitService.AnyHabitsExistForUser(dto.Id))
        {
            HabitHelpers
                .PressAnyKeyToContinue("lightgoldenrod2",
                    $"{dto.FirstName}, currently you have no habits to update. Please add a habit first");
            return;
        }

        var habits = _habitService.GetHabitsForUser(dto.Id);
        var id = HabitHelpers.GetHabitIdFromDisplay(habits, "view");

        var habit = _habitService.GetHabitForUser(dto.Id, id);

        if (habit is null)
        {
            HabitHelpers
                .PressAnyKeyToContinue("bold red", "There was an error retrieving habit for update");
            return;
        }

        var name = HabitHelpers.GetOptionalInput($"Do you wish to update the habit name?\nCurrently it is {habit.Name}")
            ? AnsiConsole.Ask<string>($"[lightslategrey]Enter updated name: [/]").Trim()
            : habit.Name;

        name = PreventHabitNameFromChangingCase(habits.Select(h => h.Name).ToList(), name);

        var dateOfOccurrence = HabitHelpers
            .GetOptionalInput(
                $"Do you wish to update the Habit date?\nCurrently it is {habit.DateOfOccurrence.ToString()}")
            ? HabitHelpers.GetDate(DateFormat)
            : habit.DateOfOccurrence;

        var unitOfMeasurement = HabitHelpers
            .GetOptionalInput(
                $"Do you wish to update the unit of measurement?\nCurrently it is {habit.UnitOfMeasurement.GetDisplayName()}")
            ? HabitHelpers.GetUnitOfMeasurement()
            : habit.UnitOfMeasurement;

        var quantity = HabitHelpers
            .GetOptionalInput(
                $"Do you wish to update the quantity of {unitOfMeasurement}?\nCurrently it is {habit.Quantity}")
            ? AnsiConsole.Ask<int>("[lightslategrey]Enter the updated quantity: [/]")
            : habit.Quantity;

        var comments = HabitHelpers
            .GetOptionalInput("Do you wish to update the comments? ")
            ? AnsiConsole.Ask<string>("[lightslategrey]Enter updated comments: [/]").Trim()
            : habit.Comments;

        var updatedHabit = new UpdateHabitDto(
            id,
            name,
            dto.Id,
            dateOfOccurrence,
            unitOfMeasurement,
            quantity,
            comments);

        HabitHelpers.PressAnyKeyToContinue("lightslategrey",
            _habitService.UpdateHabit(updatedHabit) == 1
                ? $"{dto.FirstName} habit with id {id} was updated successfully!"
                : $"Unfortunately {dto.FirstName} there was a problem updating habit with id {id}");
    }

    public void DeleteHabit(RetrievedUserDto dto)
    {
        if (!_habitService.AnyHabitsExistForUser(dto.Id))
        {
            HabitHelpers
                .PressAnyKeyToContinue("lightgoldenrod2",
                    $"{dto.FirstName}, currently you have no habits to delete. Please add a habit first");
            return;
        }

        var habits = _habitService.GetHabitsForUser(dto.Id);
        var id = HabitHelpers.GetHabitIdFromDisplay(habits, "view");

        if (!_habitService.HabitExists(dto.Id, id))
        {
            HabitHelpers.PressAnyKeyToContinue("bold red", $"Habit with id {id} not found");
            return;
        }

        HabitHelpers.PressAnyKeyToContinue("palegreen3_1",
            _habitService.DeleteHabit(dto.Id, id) == 1
                ? $"{dto.FirstName} habit with id {id} was deleted successfully"
                : $"Unfortunately {dto.FirstName} there was a problem deleting habit with id {id}");
    }

    public void ViewHabit(RetrievedUserDto dto)
    {
        if (!_habitService.AnyHabitsExistForUser(dto.Id))
        {
            HabitHelpers
                .PressAnyKeyToContinue("lightgoldenrod2",
                    $"{dto.FirstName}, currently you have no habits to view. Please add a habit first");
            return;
        }

        var habits = _habitService.GetHabitsForUser(dto.Id);
        var id = HabitHelpers.GetHabitIdFromDisplay(habits, "view");

        var habit = _habitService.GetHabitForUser(dto.Id, id);

        if (habit is null)
        {
            HabitHelpers.PressAnyKeyToContinue("bold red", $"Habit with id {id} not found");
            return;
        }

        AnsiConsole.MarkupLine($"[lightgoldenrod2]Name: [/]{habit.Name}");
        AnsiConsole.MarkupLine($"[lightgoldenrod2]Date: [/]{habit.DateOfOccurrence.ToString()}");
        AnsiConsole.MarkupLine(
            $"[lightgoldenrod2]Quantity: [/]{habit.Quantity} {habit.UnitOfMeasurement.GetDisplayName()}");
        AnsiConsole.MarkupLine($"[lightgoldenrod2]Comments: [/]{habit.Comments ?? "N/A"}");
        AnsiConsole.WriteLine();

        HabitHelpers.PressAnyKeyToContinue("lightgoldenrod2", $"Finished viewing your habit {dto.FirstName}");
    }

    public bool ViewHabits(RetrievedUserDto dto)
    {
        if (!_habitService.AnyHabitsExistForUser(dto.Id))
        {
            HabitHelpers
                .PressAnyKeyToContinue("darkslategray2",
                    $"{dto.FirstName} you currently have no habits in the database" +
                    "\nPlease add a habit first");
            return false;
        }

        var habits = _habitService.GetHabitsForUser(dto.Id);

        if (!habits.Any())
        {
            HabitHelpers
                .PressAnyKeyToContinue("bold red", "There was an error retrieving your habits");
            return false;
        }

        AnsiConsole.MarkupLine($"[navajowhite1]Habits for {dto.FirstName} {dto.LastName}[/]");

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Date");
        table.AddColumn("Unit of Measurement");
        table.AddColumn("Quantity of measurement");
        table.AddColumn("Comments");

        foreach (var habit in habits)
        {
            table.AddRow(
                $"{habit.Id}",
                $"{habit.Name}",
                $"{habit.DateOfOccurrence.ToString()}",
                $"{habit.UnitOfMeasurement.GetDisplayName()}",
                $"{habit.Quantity}",
                $"{habit.Comments ?? "N/A"}");
        }

        AnsiConsole.Write(table);

        HabitHelpers.PressAnyKeyToContinue("lightgoldenrod2", $"\nFinished viewing your habits {dto.FirstName}");

        return true;
    }

    public void ViewHabitReport(RetrievedUserDto dto)
    {
        var habitNames = _habitService.GetHabitNamesForUser(dto.Id);

        if (!habitNames.Any())
        {
            HabitHelpers
                .PressAnyKeyToContinue("lightgoldenrod2",
                    $"{dto.FirstName}, currently you have no habits to report on. Please add a habit first");
            return;
        }

        var habitName = HabitHelpers.GetHabitName(habitNames, "to view a report on");

        var habits = _habitService.GetHabitsForUser(dto.Id)
            .Where(h => h.Name.Equals(habitName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var unitsOfMeasurement = Enum.GetValues<UnitOfMeasurement>();

        var measurementMappings = new Dictionary<UnitOfMeasurement, int>();

        foreach (var unit in unitsOfMeasurement)
        {
            measurementMappings[unit] = 0;
        }

        foreach (var habit in habits)
        {
            measurementMappings[habit.UnitOfMeasurement] += habit.Quantity;
        }

        var days = habits
            .Select(h => h.DateOfOccurrence)
            .Distinct()
            .Count();

        var sessions = habits
            .Count();

        AnsiConsole.MarkupLine($"Report for {habitName}\n");
        AnsiConsole.MarkupLine($"Total days logged for habit: {days}");
        AnsiConsole.MarkupLine($"Total sessions during those {days} days: {sessions}");

        var builder = new StringBuilder();

        var units = habits
            .Select(h => h.UnitOfMeasurement)
            .Distinct()
            .ToList();

        foreach (var unit in units)
        {
            builder.Append($"{measurementMappings[unit]} {unit}\n");
        }

        var quantity = $"Total logged:\n{builder}";
        AnsiConsole.MarkupLine(quantity);

        HabitHelpers.PressAnyKeyToContinue("", "Finished viewing your report");
    }

    private static string PreventHabitNameFromChangingCase(List<string> habitNames, string habitName)
    {
        var habitAlreadyAddedWithDifferentCase = habitNames
            .Any(h => h.Equals(habitName, StringComparison.OrdinalIgnoreCase));

        if (habitAlreadyAddedWithDifferentCase)
        {
            habitName = habitNames
                .First(h => h.Equals(habitName, StringComparison.OrdinalIgnoreCase));
        }

        return habitName;
    }
}