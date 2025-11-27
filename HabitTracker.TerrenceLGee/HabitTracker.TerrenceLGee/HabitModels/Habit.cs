namespace HabitTracker.TerrenceLGee.HabitModels;

public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateOnly DateOfOccurrence { get; set; }
    public int Quantity { get; set; }
    public UnitOfMeasurement UnitOfMeasurement { get; set; }
    public string? Comments { get; set; }
}